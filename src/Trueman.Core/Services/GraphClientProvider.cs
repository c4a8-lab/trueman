using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Trueman.Core.Config;

namespace Trueman.Core.Services
{
    public class GraphClientProvider : IGraphClientProvider
    {
        private readonly AppConfig _appConfig;

        public GraphClientProvider(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }
        public GraphServiceClient CreateGraphServiceClient()
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                            .Create(_appConfig.TenantConfig.ClientId)
                            .WithTenantId(_appConfig.TenantConfig.TenantId)
                            .WithClientSecret(_appConfig.TenantConfig.ClientSecret)
                            .Build();

            ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);

            GraphServiceClient graphServiceClient = new GraphServiceClient(authenticationProvider);
            return graphServiceClient;
        }
    }
}
