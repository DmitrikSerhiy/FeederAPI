using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Feeder.DAL;
using Feeder.Infrastructure;
using Freeder.BLL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Feeder
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            autoMapperInitializatior = new AutoMapperInitializatior();
        }

        public IConfiguration Configuration { get; }
        private DependencyResolver dependencyResolver;
        private AutoMapperInitializatior autoMapperInitializatior;

        public void ConfigureServices(IServiceCollection services)
        {
            dependencyResolver = new DependencyResolver(services);

            services.AddDbContext<FeedContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc();
            //    (routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=GetRssFeed}");
            //});
        }
    }
}
