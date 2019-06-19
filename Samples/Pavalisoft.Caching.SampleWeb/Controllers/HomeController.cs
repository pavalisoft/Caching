/* 
   Copyright 2019 Pavalisoft

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.SampleWeb.Models;

namespace Pavalisoft.Caching.SampleWeb.Controllers
{
    public class HomeController : Controller
    {
        private const string CachePartitionName = "FrequentData";
        private readonly ICacheManager _cacheManager;
        public HomeController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public IActionResult Index()
        {
            AppUser appUser = GetAppUser(HttpContext);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private AppUser GetAppUser(HttpContext httpContext)
        {
            var userName = httpContext.User.Identity.Name;
            AppUser appUser;

            // Try to get the appUser from cache
            if (!_cacheManager.TryGetValue(CachePartitionName, userName, out appUser))
            {
                // If not available in Cache then create new instance of AppUser
                appUser = new AppUser(userName);

                // Add appUser object to Cache
                _cacheManager.Set(CachePartitionName, userName, appUser);                
			}
            return appUser;
        }
    }

    [Serializable]
    internal class AppUser
    {
        public AppUser(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
