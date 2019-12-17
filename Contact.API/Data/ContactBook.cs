using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Data
{
    /// <summary>
    /// 通讯录
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ContactBook
    {
        public ContactBook()
        {
            Contacts = new List<Contact>();
        }

        [BsonId]
        public ObjectId ObjectId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 联系人列表
        /// </summary>
        public List<Contact> Contacts { get; set; }
    }
}