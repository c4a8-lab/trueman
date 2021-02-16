using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Trueman.Core.Services;
using Trueman.Web.Models;

namespace Trueman.Web.Pages
{
    public partial class SetPassword
    {
        [Inject]
        public UserGraphService UserGraphService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public SetPasswordViewModel Model { get; set; } = new SetPasswordViewModel();

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;

        protected async Task HandleValidSubmit()
        {
            Message = string.Empty;
            StatusClass = string.Empty;
            try
            {
                await UserGraphService.SetUserPasswordAsync(Model.Password);
                NavigationManager.NavigateTo("/thank-you");
            }
            catch (System.Exception ex)
            {
                StatusClass = "alert-danger";
                Message = "An error occurred while setting the password";
            }
        }
        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }
    }
}
