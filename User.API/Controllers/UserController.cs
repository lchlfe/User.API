using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using User.API.Data;
using User.API.Dtos;
using User.API.IntergrationEvents;
using User.API.Models;

namespace User.API.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserDbContext _dbContext;
        private readonly ILogger<UserController> _logger;
        private readonly ICapPublisher _capPublisher;

        public UserController(UserDbContext dbContext, ILogger<UserController> logger, ICapPublisher capPublisher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _capPublisher = capPublisher;
        }
        //发布消息
        private async Task RasieUserInfoChangedEventAsyncTask(AppUser user)
        {
            //判断值是否更改
            if (_dbContext.Entry(user).Property(x => x.Name).IsModified ||
                _dbContext.Entry(user).Property(x => x.Company).IsModified ||
                _dbContext.Entry(user).Property(x => x.Title).IsModified ||
                _dbContext.Entry(user).Property(x => x.Phone).IsModified ||
                _dbContext.Entry(user).Property(x => x.Avatar).IsModified)
            {
                var @event = new AppUserInfoChangedEvent()
                {
                    Avatar = user.Avatar,
                    Company = user.Company,
                    Id = user.Id,
                    Name = user.Name,
                    Phone = user.Phone,
                    Title = user.Title
                };
                await _capPublisher.PublishAsync<AppUserInfoChangedEvent>("userapi.userinfochanged", @event);
            }
        }
        /// <summary>
        /// 获取个人资料
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var user = await _dbContext.AppUsers
                .AsNoTracking()
                .Include(x => x.Properties)
                .SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            if (user == null)
            {

                throw new UserOperationException($"错误的用户上下文Id {UserIdentity.UserId}");
            }
            return Json(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<AppUser> patch)
        {
            //视频【65】
            //有数组的情况下，更新有点复杂，
            //1先删除原先的在做新增操作
            //2.匹配后，在做关联操作，（不要跟踪数据）
            var user = await _dbContext.AppUsers
                .SingleOrDefaultAsync(x => x.Id == UserIdentity.UserId);
            if (user == null)
            {
                throw new UserOperationException($"错误的用户上下文Id {UserIdentity.UserId}");
            }
            patch.ApplyTo(user);

            foreach (var item in user?.Properties)
            {
                _dbContext.Entry(item).State = EntityState.Detached;
            }

            var currentPros = user.Properties;
            var originPros = await _dbContext.AppUserProperties.AsNoTracking().Where(x => x.AppUserId == user.Id).ToListAsync();
            var allPros = originPros.Union(currentPros).Distinct();

            var removeRang = originPros.Except(currentPros);
            _dbContext.RemoveRange(removeRang);

            var addRang = allPros.Except(originPros);

            using (var trans = await _dbContext.Database.BeginTransactionAsync())
            {
                //发布用户变更的消息
                await RasieUserInfoChangedEventAsyncTask(user);

                await _dbContext.AddRangeAsync(addRang);

                await _dbContext.SaveChangesAsync();

                trans.Commit();
            }

            return Json(user);
        }

        /// <summary>
        /// 验证用户是否存在，如果不存在则创建，否则返回
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("check-or-create")]
        public async Task<IActionResult> CheckOrCreate([FromForm]string phone)
        {
            // todo: phone验证
            var user = await _dbContext.AppUsers.SingleOrDefaultAsync(x => x.Phone == phone);

            if (user != null) return Ok(user);
            user = new AppUser()
            {
                Phone = phone
            };
            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Ok(user);
        }
        /// <summary>
        /// 获取用户标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetTags()
        {
            var result = await _dbContext.AppUserTags.Where(x => x.AppUserId == UserIdentity.UserId).ToListAsync();
            return Ok(result);
        }
        /// <summary>
        /// 通过手机号查找用户资料
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search([FromForm]string phone)
        {
            var appUser = await _dbContext.AppUsers.Include(x => x.Properties).SingleOrDefaultAsync(x => x.Phone == phone);
            return Json(appUser);
        }
        /// <summary>
        /// 更新用户标签
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-tags")]
        public async Task<IActionResult> UpdateTags([FromBody]string[] tags)
        {
            tags = tags ?? new string[] { };
            var originList = await _dbContext.AppUserTags.Where(x => x.AppUserId == UserIdentity.UserId).Select(x => x.Tag).ToListAsync();
            var removeTags = originList.Except(tags).ToList();
            var addTags = tags.Except(originList).ToList();
            await _dbContext.AddRangeAsync(addTags.Select(x => new AppUserTag()
            {
                AppUserId = UserIdentity.UserId,
                Tag = x,
                CreatedTime = DateTime.Now
            }));
            var removeList = await _dbContext.AppUserTags.Where(x => x.AppUserId == UserIdentity.UserId)
                .Where(x => removeTags.Contains(x.Tag)).ToListAsync();
            _dbContext.RemoveRange(removeList);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// 获取好友用户信息和标签
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("baseinfo/{userid}")]
        public async Task<IActionResult> BaseInfo(int userid)
        {
            var appUser = await _dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == userid);
            if (appUser == null) return NotFound();
            var baseInfo = new BaseUserInfo()
            {
                Avatar = appUser.Avatar,
                Company = appUser.Company,
                Name = appUser.Name,
                Phone = appUser.Phone,
                Title = appUser.Title,
                UserId = appUser.Id
            };
            baseInfo.Tags = await _dbContext.AppUserTags.Where(x => x.AppUserId == appUser.Id).Select(x => x.Tag)
                .ToArrayAsync() ?? new string[] { };
            return Ok(baseInfo);
        }
    }
}
