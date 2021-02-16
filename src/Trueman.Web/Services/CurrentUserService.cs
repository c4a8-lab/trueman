using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using Trueman.Core.Services;

namespace Trueman.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }
        public string UserPrincipalName
        {
            get
            {
                return _authenticationStateProvider.GetAuthenticationStateAsync().GetAwaiter().GetResult().User.Identity.Name;
            }
        }

        public string DisplayName
        {
            get
            {
                return _authenticationStateProvider.GetAuthenticationStateAsync().GetAwaiter().GetResult().User.Claims.First(x => x.Type.Equals("name")).Value;
            }
        }
    }
}
