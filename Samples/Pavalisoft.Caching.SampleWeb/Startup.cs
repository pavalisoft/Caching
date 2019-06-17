using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//Import the below namespace to use InMemory and DitributedInMemory cache store implementations
using Pavalisoft.Caching.InMemory;
//Import the below namespace to use MySql cache store implementation
using Pavalisoft.Caching.MySql;
//Import the below namespace to use Redis Cache Store implementation
using Pavalisoft.Caching.Redis;
//Import the below namespace to use SqlServer Cache Store implementation
using Pavalisoft.Caching.SqlServer;

namespace Pavalisoft.Caching.SampleWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Adds CacheManager servcice to services
            services.AddCaching()
                // Adds InMemory and Distributed InMemory Cache Store implementations to CacheManager
                .AddInMemoryCache()
                // Adds MySql Cache Store implementations to CacheManager
                .AddMySqlCache()
                // Adds Redis Cache Store implementations to CacheManager
                .AddRedisCache()
                // Adds SqlServer Cache Store implementations to CacheManager
                .AddSqlServerCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
