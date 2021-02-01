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

namespace Trueman.Web.Pages
{
    public partial class Index
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        [Inject]
        public GraphServiceClient GraphServiceClient { get; set; }
        [Inject]
        public MicrosoftIdentityConsentAndConditionalAccessHandler MicrosoftIdentityConsentAndConditionalAccessHandler { get; set; }

        public User User { get; set; }
        public string Photo { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var state = await AuthState;
            ClaimsPrincipal = state.User;
            try
            {
                var user = await GraphServiceClient
                    .Me
                    .Request()
                    .Select(user => new
                    {
                        user.Photo,
                        user.DisplayName,
                        user.GivenName,
                        user.Surname,
                        user.OfficeLocation,
                        user.UsageLocation,
                        user.UserPrincipalName
                    })
                    .GetAsync();
                User = user;
                try
                {
                    using (var photoStream = await GraphServiceClient.Me.Photo.Content.Request().GetAsync())
                    {
                        byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                        Photo = Convert.ToBase64String(photoByte);
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                MicrosoftIdentityConsentAndConditionalAccessHandler.HandleException(ex);
            }
        }
    }
}
