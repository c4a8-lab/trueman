using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trueman.Core.Services
{
    public class WindowsAutopilotGraphService
    {
        private readonly IGraphClientProvider _graphClientProvider;

        public WindowsAutopilotGraphService(IGraphClientProvider graphClientProvider)
        {
            _graphClientProvider = graphClientProvider;
        }

        public async Task<List<WindowsAutopilotDeviceIdentity>> GetAllWindowsAutopilotDevicesAsync()
        {
            //Graph API doesn't support filter on all properties of windowsAutopilotDeviceIdentity object. This might change in future. So far now we fetch all of them

            var allDevices = new List<WindowsAutopilotDeviceIdentity>();

            var autopilotDevices = await GraphClient
                .DeviceManagement
                .WindowsAutopilotDeviceIdentities
                .Request()
                .Top(998)
                .GetAsync();

            while (autopilotDevices.Count > 0)
            {
                allDevices.AddRange(autopilotDevices.ToList());

                if (autopilotDevices.NextPageRequest != null)
                {
                    autopilotDevices = await autopilotDevices.NextPageRequest.GetAsync();
                }
                else
                {
                    break;
                }
            }
            return allDevices;
        }

        public async Task<WindowsAutopilotDeviceIdentity> GetWindowsAutopilotDeviceAsync(string serialNumber)
        {
            var response = await GraphClient
                .DeviceManagement
                .WindowsAutopilotDeviceIdentities
                .Request()
                .Filter($"contains(serialNumber,'{serialNumber}')")
                .GetAsync();

            if(response.Count > 1)
            {
                throw new ApplicationException($"More than one device found with serial number {serialNumber}");
            }
            return response.FirstOrDefault();
        }

        public async Task AssignUserToDeviceAsync(string autopilotDeviceIdentityId, string userPrincipalName, string addressableUserName)
        {
            await GraphClient
                .DeviceManagement
                .WindowsAutopilotDeviceIdentities[autopilotDeviceIdentityId]
                .AssignUserToDevice(userPrincipalName, addressableUserName)
                .Request()
                .PostAsync();

        }

        private GraphServiceClient GraphClient => _graphClientProvider.CreateGraphServiceClient();

    }
}
