using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Trueman.Core.Extensions
{
    public static class LoggerExtensions
    {
        public static async Task LogHttpResponseAsync(this ILogger logger, HttpResponseMessage httpResponseMessage)
        {
            var responseToLog = new
            {
                statusCode = httpResponseMessage.StatusCode,
                content = await httpResponseMessage.Content.ReadAsStringAsync(),
                headers = httpResponseMessage.Headers,
                reasonPhrase = httpResponseMessage.ReasonPhrase,
                errorMessage = httpResponseMessage.RequestMessage
            };
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //Comment
                logger.LogInformation($"Response is {JsonConvert.SerializeObject(responseToLog)}");

            }
            else
            {
                logger.LogError($"Response is {JsonConvert.SerializeObject(responseToLog)}");
            }
        }
    }
}
