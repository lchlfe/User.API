using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using DotNetCore.CAP.MySql;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using User.API.Controllers;
using User.API.Data;
using User.API.Models;
using Xunit;

namespace User.API.UnitTests
{
    public class UserControllerUnitTests
    {
        //��ӳ�ʼ����,�ڴ��Ե����ݿ�
        private UserDbContext GetUserDbContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new UserDbContext(options);

            dbContext.AppUsers.Add(new AppUser()
            {
                Id = 1,
                Name = "djlnet"
            });

            dbContext.SaveChanges();
            return dbContext;
        }

        //private (Controllers.UserController controller,Data.UserDbContext userContext,ICapPublisher pu) GetUserController()
        //{
        //    var context = GetUserDbContext();
        //    var loggerMoq = new Mock<ILogger<API.Controllers.UserController>>();
        //     var publisher = new Mock<ICapPublisher>().Object;
        //    var logger = loggerMoq.Object;
        //    return (controller: new Controllers.UserController(context, logger, publisher), userContext: context,pu:publisher);
        //}


        //��ȡ�û�����
        [Fact]
        public void Get_ReturnRightUser_WithExpectParamters()
        {
            //�ع�����
            //(UserController controler, UserDbContext usercontext, ICapPublisher pu) = GetUserController();

            var dbContext = GetUserDbContext();
            var loggerMock = new Mock<ILogger<UserController>>();
            var logger = loggerMock.Object;
            var publisher = new Mock<ICapPublisher>().Object;
            var controler = new UserController(dbContext, logger, publisher);

            var result = controler.Get().Result;
            //Assert.IsType<JsonResult>(result);

            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            var appUser = jsonResult.Value.Should().BeAssignableTo<AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("djlnet");
        }

        #region ��ʱ�д���
        //[Fact]
        //public async Task Patch_ReturnNewName_WithParamter()
        //{
        //    var dbContext = GetUserDbContext();
        //    var loggerMock = new Mock<ILogger<UserController>>();
        //    var logger = loggerMock.Object;
        //    var publisher = new Mock<ICapPublisher>().Object;
        //    var controler = new UserController(dbContext, logger, publisher);

        //    var doc = new JsonPatchDocument<AppUser>();
        //    doc.Replace(x => x.Name, "djlnet");
        //    var reponse = controler.Patch(doc);
        //    var result = reponse.Result.Should().BeOfType<JsonResult>().Subject;

        //    var appuser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
        //    appuser.Name.Should().Be("djlnet");

        //    var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == 1);
        //    user.Should().NotBeNull();
        //    user.Name.Should().Be("djlnet");
        //}

        //[Fact]
        //public async Task Patch_ReturnNewProperties_WithAddNewPropertyParamter()
        //{
        //    var dbContext = GetUserDbContext();
        //    var loggerMock = new Mock<ILogger<UserController>>();
        //    var logger = loggerMock.Object;
        //    var publisher = new Mock<ICapPublisher>().Object;
        //    var controler = new UserController(dbContext, logger, publisher);
        //    var doc = new JsonPatchDocument<AppUser>();
        //    doc.Replace(x => x.Properties, new List<AppUserProperty>()
        //    {
        //        new AppUserProperty()
        //        {
        //            Key = "fin",Value = "fuck",Text = "fuck"
        //        }
        //    });
        //    var reponse = controler.Patch(doc);
        //    var result = reponse.Result.Should().BeOfType<JsonResult>().Subject;
        //    var appuser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
        //    appuser.Properties.Count.Should().Be(1);
        //    appuser.Properties.First().Value.Should().Be("fuck");
        //    appuser.Properties.First().Key.Should().Be("fin");
        //    appuser.Properties.First().Text.Should().Be("fuck");

        //    var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == 1);
        //    user.Should().NotBeNull();
        //    user.Properties.First().Value.Should().Be("fuck");
        //    user.Properties.First().Key.Should().Be("fin");
        //    user.Properties.First().Text.Should().Be("fuck");
        //}

        //[Fact]
        //public async Task Patch_ReturnEmptyProperties_WithRemovePropertyParamter()
        //{
        //    var dbContext = GetUserDbContext();
        //    var loggerMock = new Mock<ILogger<UserController>>();
        //    var logger = loggerMock.Object;
        //    var publisher = new Mock<ICapPublisher>().Object;
        //    var controler = new UserController(dbContext, logger, publisher);
        //    var doc = new JsonPatchDocument<AppUser>();
        //    doc.Replace(x => x.Properties, new List<AppUserProperty>());
        //    var reponse = controler.Patch(doc);
        //    var result = reponse.Result.Should().BeOfType<JsonResult>().Subject;
        //    var appuser = result.Value.Should().BeAssignableTo<AppUser>().Subject;
        //    appuser.Properties.Should().BeEmpty();

        //    var user = await dbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == 1);
        //    user.Should().NotBeNull();
        //    user.Properties.Count.Should().Be(0);
        //} 
        #endregion
    }
}
