using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LoggerCallbackHandler.Callback = new MyAdalLoggerCallback();
            MakeRequest();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string GetToken()
        {
            string authority = "https://login.microsoftonline.com/adfei.onmicrosoft.com/";
            string clientId = "eca61fd9-f491-4f03-a622-90837bbc1711";
            string secret = "vIAI4mnMTIj6HtT1AC8V8KPoUdx4ecu3UBN3HAmw71g=";
            string resource = "https://graph.windows.net/";

            var credential = new ClientCredential(clientId, secret);
            AuthenticationContext authContext = new AuthenticationContext(authority);
            
            //I think the problem is here:
            var token = authContext.AcquireTokenAsync(resource, credential).Result.AccessToken;

            return token;
        }

        public string MakeRequest()
        {
            string accessToken = GetToken();
            var tenantId = "adfei.onmicrosoft.com";
            string graphResourceId = "https://graph.windows.net/";

            Uri servicePointUri = new Uri(graphResourceId);
            Uri serviceRoot = new Uri(servicePointUri, tenantId);

            ActiveDirectoryClient client = new ActiveDirectoryClient(serviceRoot, async () => await Task.FromResult(accessToken));

            foreach (var user in client.Users.ExecuteAsync().Result.CurrentPage)
                Console.WriteLine(user.DisplayName);

            var client1 = new HttpClient();
            var uri = "https://graph.windows.net/" + tenantId + "/users?api-version=1.6";
            client1.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = client1.GetAsync(uri).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }
    }

    public class MyAdalLoggerCallback : IAdalLogCallback
    {
        public void Log(LogLevel level, string message)
        {
            //Console.WriteLine(message);
            Debug.WriteLine(message);
            // platform-specific code goes here
        }
    }
}