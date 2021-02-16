using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trueman.Core.Models;

namespace Trueman.Core.Services
{
    public class UserDeviceManagementService
    {
        private readonly WindowsAutopilotGraphService _windowsAutopilotGraphService;
        private readonly ICurrentUserService _currentUserService;

        public UserDeviceManagementService(WindowsAutopilotGraphService windowsAutopilotGraphService, ICurrentUserService currentUserService)
        {
            _windowsAutopilotGraphService = windowsAutopilotGraphService;
            _currentUserService = currentUserService;
        }

        public async Task<List<WindowsAutopilotDeviceIdentity>> GetWindowsAutopilotDevicesAssignedToUserAsync()
        {
            var allDevices = await _windowsAutopilotGraphService.GetAllWindowsAutopilotDevicesAsync();

            return allDevices.Where(x => x.UserPrincipalName.Equals(_currentUserService.UserPrincipalName)).ToList();
        }

        public async Task<WindowsAutopilotDevicesAssignedToUserDto> GetWindowsAutopilotDevicesAssignedToUserDtoAsync()
        {
            var assignedDevices = await GetWindowsAutopilotDevicesAssignedToUserAsync();
            return new WindowsAutopilotDevicesAssignedToUserDto
            {
                Items = assignedDevices
            };
        }
        public async Task<WindowsAutopilotDeviceStatus> GetWindowsAutopilotDeviceStatusAsync(string serialNumber)
        {
            var device = await _windowsAutopilotGraphService.GetWindowsAutopilotDeviceAsync(serialNumber);
            return new WindowsAutopilotDeviceStatus(_currentUserService.UserPrincipalName, device);
        }

        public async Task<AssignDeviceToUserResponse> AssignDeviceToUserAsync(string serialNumber)
        {
            var device = await GetWindowsAutopilotDeviceStatusAsync(serialNumber);
            if (device.Found && !device.Enrolled && !device.AssignedToThisUser && !device.AssignedToOtherUser)
            {
                await _windowsAutopilotGraphService.AssignUserToDeviceAsync(device.WindowsAutopilotDeviceIdentity.Id, _currentUserService.UserPrincipalName, _currentUserService.DisplayName);
                return new AssignDeviceToUserResponse
                {
                    Success = true
                };
            }
            string errorMessage = "Unknown";
            if (!device.Found)
            {
                errorMessage = $"Device #{serialNumber} not found";
            }
            if (device.Enrolled)
            {
                errorMessage = $"Device #{serialNumber} already enrolled";
            }
            if (device.AssignedToOtherUser || device.AssignedToThisUser)
            {
                errorMessage = $"Device #{serialNumber} already assigned";
            }
            return new AssignDeviceToUserResponse
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
