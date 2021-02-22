namespace Trueman.Core.Clients.TemporaryAccessPass
{
    public class TapToCreateDto
    {
        public string UserPrincipalName { get; set; }
        public bool IsUsableOnce { get; set; }
    }
}
