using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Services
{
    public class AssetLocator
    {
        private readonly IHostingEnvironment _env;

        public AssetLocator(IHostingEnvironment env)
        {
            _env = env;
        }

        public string GetAsset(string keyName)
        {
            using (StreamReader reader = new StreamReader($"{_env.WebRootPath}/build/manifest.json"))
            {
                var json = reader.ReadToEnd();
                var mapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return mapping[keyName];
            }
        }
    }
}
