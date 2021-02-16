namespace Trueman.Core.Services
{
    public interface ICurrentUserService
    {
        string UserPrincipalName { get; }
        string DisplayName { get; }
    }
}
