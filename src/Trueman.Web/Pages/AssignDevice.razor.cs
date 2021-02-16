using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Trueman.Core.Models;
using Trueman.Core.Services;
using Trueman.Web.Models;

namespace Trueman.Web.Pages
{
    public partial class AssignDevice
    {
        [Inject]
        public UserDeviceManagementService UserDeviceManagementService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public WindowsAutopilotDevicesAssignedToUserDto WindowsAutopilotDevicesAssignedToUserDto { get; set; }

        public AssignDeviceViewModel Model { get; set; } = new AssignDeviceViewModel();

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        //protected override async Task OnInitializedAsync()
        //{
        //    //var windowsAutopilotDevicesAssignedToUserDto = await UserDeviceManagementService.GetWindowsAutopilotDevicesAssignedToUserDtoAsync();
        //    //WindowsAutopilotDevicesAssignedToUserDto = windowsAutopilotDevicesAssignedToUserDto;
        //}

        protected async Task HandleValidSubmit()
        {
            Message = string.Empty;
            StatusClass = string.Empty;

            var response = await UserDeviceManagementService.AssignDeviceToUserAsync(Model.SerialNumber);
            if (response.Success)
            {
                NavigationManager.NavigateTo("/set-password");
            }
            else
            {
                StatusClass = "alert-danger";
                Message = response.ErrorMessage;
            }
        }
        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }
    }
}
