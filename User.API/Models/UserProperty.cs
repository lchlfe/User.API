using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    /// <summary>
    /// 用户属性（）
    /// </summary>
    public class AppUserProperty
    {
        private int? _requestedHashCode;
        /// <summary>
        /// 用户Id（会自动建立映射）
        /// </summary>
        public int AppUserId { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        ///Value
        /// </summary>
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AppUserProperty))
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
                return true;
            AppUserProperty item = (AppUserProperty)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
            {
                return item.Key == this.Key && item.Value == this.Value;
            }
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = (this.Key + this.Value).GetHashCode() ^ 31;
                }
                return _requestedHashCode.Value;
            }
            return base.GetHashCode();
        }

        public bool IsTransient()
        {
            return string.IsNullOrEmpty(this.Key) || string.IsNullOrEmpty(this.Value);
        }
    }
}
