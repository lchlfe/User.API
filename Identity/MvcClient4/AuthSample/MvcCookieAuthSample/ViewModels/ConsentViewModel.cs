﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.ViewModels
{
    public class ConsentViewModel:InputConsentViewModel
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        //public bool RememberConsent { get; set; }
        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
        //public string ReturnUrl { get; set; }
    }

    public class ScopesInput
    {
        public List<string> ScopesConsented { get; set; }
    }
}
