using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample4.Models
{
    public class InputConsentViewModel
    {
        public string Button { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberConsent { get; set; }

        public IEnumerable<string> ScopesConsentId { get; set; }
    }
}
