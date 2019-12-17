using System;
using Contact.API.Data.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contact.API.Data
{
    /// <summary>
    /// 好友申请模型
    /// </summary>
    [BsonIgnoreExtraElements]
    public class FriendRequest
    {
        public FriendRequest()
        {
            ApplyStatus = ApplyStatus.Waiting;
            ApplyDateTime = DateTime.Now;
        }

        [BsonId]
        public ObjectId ObjectId { get; set; }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 好友用户ID
        /// </summary>
        public int AppliedUserId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public ApplyStatus ApplyStatus { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ApplyDateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}