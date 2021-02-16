using Microsoft.Graph;

namespace Trueman.Core.Models
{
    public class UserInfoDto
    {
        public User User { get; set; }
        public string PictureBase64 { get; set; }
    }
}
