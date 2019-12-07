using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MvcCookieAuthSample.ViewModels;

namespace MvcCookieAuthSample.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore
            , IResourceStore resourceStore
            , IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl,InputConsentViewModel model=null)
        {
            AuthorizationRequest request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }

            Client client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            Resources resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var vm = CreateConsentViewModel(request, client, resources,model);
            vm.ReturnUrl = returnUrl;
            return vm;
        }

        public async Task<ProcessConsentResult> ProcessConsent(InputConsentViewModel model)
        {
            ConsentResponse consentResponse = null;
            var result=new ProcessConsentResult();
            if (model.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (model.Button == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    consentResponse = new ConsentResponse()
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented
                    };
                }
                else
                {
                    result.ValidationError = "请至少选择一个权限";
                }
            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);
                result.RedirectUrl = model.ReturnUrl;
            }
            else
            {
                ConsentViewModel consentViewModel = await BuildConsentViewModel(model.ReturnUrl,model);
                result.ViewModel = consentViewModel;
            }

            return result;
        }

        #region Private Methods

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client,
            Resources resources,InputConsentViewModel model)
        {
            var rememberConsent = model?.RememberConsent ?? true;
            var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();

            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.RememberConsent = rememberConsent;

            vm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i,selectedScopes.Contains(i.Name)||model==null));
            vm.ResourceScopes = resources.ApiResources.SelectMany(i => i.Scopes).Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name)||model==null));

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource,bool check)
        {
            return new ScopeViewModel()
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = check|| identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel()
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = check||scope.Required,
                Emphasize = scope.Emphasize
            };
        }
        #endregion
    }
}
