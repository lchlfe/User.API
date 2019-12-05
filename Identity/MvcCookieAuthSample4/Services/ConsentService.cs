using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MvcCookieAuthSample4.Models;

namespace MvcCookieAuthSample4.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private ScopeViewModel CreateScopViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel()
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopViewModel(Scope scope)
        {
            return new ScopeViewModel()
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = scope.Required,
                Emphasize = scope.Emphasize
            };
        }

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null) return null;

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            if (client == null) return null;

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            if (resources == null) return null;

            var consentViewModel = new ConsentViewModel();
            consentViewModel.ClientName = client.ClientName;
            consentViewModel.ClientLogoUrl = client.LogoUri;
            consentViewModel.ClientUrl = client.ClientUri;
            consentViewModel.AllowRememberConsent = client.AllowRememberConsent;

            consentViewModel.IdentityScopes = resources.IdentityResources.Select(p => CreateScopViewModel(p));
            consentViewModel.ResourceScopes = resources.ApiResources.SelectMany(p => p.Scopes).Select(p => CreateScopViewModel(p));

            consentViewModel.ReturnUrl = returnUrl;

            return consentViewModel;
        }


    }
}
