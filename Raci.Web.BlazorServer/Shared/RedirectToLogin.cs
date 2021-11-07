using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Raci.Web.BlazorServer.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            try
            {
                //var parameters = new Dictionary<string, string>();
                //parameters.Add("redirectUrl", NavigationManager.Uri);

                //// This code will lead the user to hips v1 to retrieve the access token
                //var uriWithQuery = QueryHelpers.AddQueryString(NavigationManager.BaseUri + "login/login-from-hips-cookie", parameters);

                //// This code will lead the user to login page of hips v2
                ////var uriWithQuery = QueryHelpers.AddQueryString(NavigationManager.BaseUri + "sign-in", parameters);

                //NavigationManager.NavigateTo(uriWithQuery, true);

                NavigationManager.NavigateTo("/");
            }
            catch
            {
            }

        }
    }
}
