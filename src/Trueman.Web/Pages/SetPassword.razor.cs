using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using Trueman.Core.Clients.TemporaryAccessPass;
using Trueman.Core.Services;
using Trueman.Web.Models;

namespace Trueman.Web.Pages
{
    public partial class SetPassword
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; }

        [Inject]
        public UserGraphService UserGraphService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public TapData Response { get; set; }

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthState;

            Message = string.Empty;
            StatusClass = string.Empty;
            try
            {
                Response = await UserGraphService.SetTapAsync(state.User.Identity.Name);
            }
            catch (System.Exception ex)
            {
                StatusClass = "alert-danger";
                Message = "An error occurred while setting the temporary access password for you.";
            }
            await base.OnInitializedAsync();
        }

        protected void TryAgain()
        {
            NavigationManager.NavigateTo("/set-password", true);
        }
    }
}
