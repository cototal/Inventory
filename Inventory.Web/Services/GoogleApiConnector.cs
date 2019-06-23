using Inventory.Web.Config;
using Inventory.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Services
{
    public class GoogleApiConnector
    {
        private readonly GoogleApi _config;

        public GoogleApiConnector(GoogleApi config)
        {
            _config = config;
        }

        public string LoginRequestPath()
        {
            var req = new string[] {
                "https://accounts.google.com/o/oauth2/auth",
                "?response_type=code&access_type=offline",
                $"&client_id={_config.ClientId}",
                $"&redirect_uri={_config.RedirectUri}",
                "&state&scope=email%20profile&approval_prompt=auto&include_granted_scopes=true"
            };
            return string.Join("", req);
        }

        public async Task<string> TokenExchange(string code)
        {
            var requestBody = new Dictionary<string, string>()
            {
                {"code", code },
                {"client_id", _config.ClientId },
                {"client_secret", _config.ClientSecret },
                {"redirect_uri", _config.RedirectUri },
                {"grant_type", "authorization_code" }
            };
            var client = new HttpClient();
            var postMessage = new FormUrlEncodedContent(requestBody);
            var response = await client.PostAsync("https://www.googleapis.com/oauth2/v4/token", postMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
            return data["access_token"];
            // 
        }

        public async Task<GoogleUserObject> GoogleUser(string accessToken)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"https://www.googleapis.com/userinfo/v2/me?access_token={accessToken}");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GoogleUserObject>(responseBody);
        }
    }
}
