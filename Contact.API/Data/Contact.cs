namespace Contact.API.Data
{
    /// <summary>
    /// 好友
    /// </summary>
    public class Contact
    {
        public Contact()
        {
            Tags = new string[] { };
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
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

        public string[] Tags { get; set; }
    }
}