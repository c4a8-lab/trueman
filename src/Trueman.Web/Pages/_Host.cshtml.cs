using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trueman.Web.Pages
{
    public class _HostModel : PageModel
    {
        private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _conditionalAccessHandler;
        private readonly GraphServiceClient _graphServiceClient;

        public _HostModel(MicrosoftIdentityConsentAndConditionalAccessHandler conditionalAccessHandler, GraphServiceClient graphServiceClient)
        {
            _conditionalAccessHandler = conditionalAccessHandler;
            _graphServiceClient = graphServiceClient;

        }
        public IActionResult OnGet()
        {
            //if (User.Identity.IsAuthenticated)
            //{

            //    try
            //    {
            //        var user = await _graphServiceClient.Me.Request().GetAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        //can't get a token from the token store, MUST assume a sign-out path as requests to API will NOT be authenticated
            //        await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { });
            //    }
            //}

            return Page();
        }
    }
}
