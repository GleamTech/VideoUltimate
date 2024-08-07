using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GleamTech.AspNet;
using GleamTech.AspNet.Core;
using GleamTech.VideoUltimate;

namespace GleamTech.VideoUltimateExamples.AspNetCoreCS
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
            services.AddControllersWithViews();


            //----------------------
            //Add GleamTech to the ASP.NET Core services container.
            services.AddGleamTech();
            //----------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            //----------------------
            //Register GleamTech to the ASP.NET Core HTTP request pipeline.
            app.UseGleamTech(() =>
            {
                //The below custom config file loading is only for our demo publishing purpose:

                var gleamTechConfig = Hosting.ResolvePhysicalPath("~/App_Data/GleamTech.config");
                if (File.Exists(gleamTechConfig))
                    GleamTechConfiguration.Current.Load(gleamTechConfig);

                var videoUltimateConfig = Hosting.ResolvePhysicalPath("~/App_Data/VideoUltimate.config");
                if (File.Exists(videoUltimateConfig))
                    VideoUltimateConfiguration.Current.Load(videoUltimateConfig);
            });
            //----------------------


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
