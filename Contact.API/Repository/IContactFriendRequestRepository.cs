using System.Collections.Generic;
using System.Threading.Tasks;
using Contact.API.Data;

namespace Contact.API.Repository
{
    public interface IContactFriendRequestRepository
    {
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<FriendRequest>> GetFriendRequestListAsync(int userId);
        /// <summary>
        /// 添加申请好友的请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddFriendAsync(FriendRequest request);
        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appliedUserId"></param>
        /// <returns></returns>
        Task PassFriendRequestAsync(int userId, int appliedUserId);
        /// <summary>
        /// 拒绝好友申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appliedUserId"></param>
        /// <returns></returns>
        Task RejectFriendRequestAsync(int userId, int appliedUserId);
        /// <summary>
        /// 检查好友申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appliedUserId"></param>
        /// <returns></returns>
        Task<bool> ExistFriendRequestAsync(int userId, int appliedUserId);
    }
}