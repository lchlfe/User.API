using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class AppUserTag
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public int AppUserId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string Tag { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
