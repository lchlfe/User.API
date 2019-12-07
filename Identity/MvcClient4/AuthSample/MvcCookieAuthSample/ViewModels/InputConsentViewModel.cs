using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.ViewModels
{
    public class InputConsentViewModel
    {
        /// <summary>
        /// 按钮
        /// </summary>
        public string Button { get; set; }
        /// <summary>
        /// 接收到的勾选的Scope
        /// </summary>
        public IEnumerable<string> ScopesConsented { get; set; }
        /// <summary>
        /// 是否选择记住
        /// </summary>
        public bool RememberConsent { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
