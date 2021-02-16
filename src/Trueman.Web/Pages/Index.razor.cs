using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trueman.Core.Models;
using Trueman.Core.Services;

namespace Trueman.Web.Pages
{
    public partial class Index
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public UserGraphService UserGraphService { get; set; }
        [Inject]
        public MicrosoftIdentityConsentAndConditionalAccessHandler MicrosoftIdentityConsentAndConditionalAccessHandler { get; set; }

        public UserInfoDto UserInfo { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthState;
            UserInfo = await UserGraphService.GetUserAsync(state.User.Identity.Name);
        }
        protected void NavigateToDeviceAssignment()
        {
            NavigationManager.NavigateTo("/assign-device");
        }
    }
}
