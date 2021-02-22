using Microsoft.Graph;
using System;
using System.IO;
using System.Threading.Tasks;
using Trueman.Core.Clients.TemporaryAccessPass;
using Trueman.Core.Models;

namespace Trueman.Core.Services
{
    public class UserGraphService
    {
        private readonly IGraphClientProvider _graphClientProvider;
        private readonly TemporaryAccessPassClient _temporaryAccessPassClient;

        public UserGraphService(IGraphClientProvider graphClientProvider, TemporaryAccessPassClient temporaryAccessPassClient)
        {
            _graphClientProvider = graphClientProvider;
            _temporaryAccessPassClient = temporaryAccessPassClient;
        }
        private GraphServiceClient GraphClient => _graphClientProvider.CreateGraphServiceClient();

        public async Task<TapData> SetTapAsync(string userPrincipalName)
        {
            var existingTap = await _temporaryAccessPassClient.GetTapFromListAsync(userPrincipalName);
            if (existingTap != null)
            {
                await _temporaryAccessPassClient.DeleteTapAsync(userPrincipalName, existingTap.Id);
            }

            var response = await _temporaryAccessPassClient.CreateTapAsync(new TapToCreateDto { UserPrincipalName = userPrincipalName, IsUsableOnce = true });
            return response;
        }

        //public async Task SetUserPasswordAsync(string password)
        //{
        //    var user = await GraphClient
        //         .Users[_currentUserService.UserPrincipalName]
        //         .Request()
        //         .UpdateAsync(new User
        //         {
        //             PasswordProfile = new PasswordProfile
        //             {
        //                 ForceChangePasswordNextSignIn = false,
        //                 ForceChangePasswordNextSignInWithMfa = false,
        //                 Password = password
        //             }
        //         });
        //}

        public async Task<UserInfoDto> GetUserAsync(string userPrincipalName)
        {
            var user = await GraphClient
                .Users[userPrincipalName]
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

            var userPicture = await GetUserPhotoAsync(userPrincipalName);


            return new UserInfoDto
            {
                PictureBase64 = userPicture,
                User = user
            };
        }
        private async Task<string> GetUserPhotoAsync(string userPrincipalName)
        {
            try
            {
                var pictureStream = await GraphClient.Users[userPrincipalName].Photos["120x120"].Content.Request().GetAsync();
                var pictureMemoryStream = new MemoryStream();
                await pictureStream.CopyToAsync(pictureMemoryStream);

                // Convert stream to byte array.
                var pictureByteArray = pictureMemoryStream.ToArray();

                // Convert byte array to base64 string.
                var pictureBase64 = Convert.ToBase64String(pictureByteArray);

                //return "data:image/jpeg;base64," + pictureBase64;
                return pictureBase64;
            }
            catch (Exception ex)
            {
                //Picture not found
                return string.Empty;
            }
        }
    }
}
