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

    internal class AppUser
    {
        public AppUser(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
