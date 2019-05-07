using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LET.Panopto.Scheduler.Models;
using Microsoft.EntityFrameworkCore;
using LET.Panopto.Scheduler.Scheduling;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace LET.Panopto.Scheduler
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //DI for Panopto services
            //session management
            services.Configure<SessionManagementAuthConfig>(Configuration.GetSection("PanoptoSessionManager"));
            services.Configure<SessionManagementPagingConfig>(Configuration.GetSection("PanoptoSessionPaging"));
            //recorder management
            services.Configure<RecorderManagementAuthConfig>(Configuration.GetSection("PanoptoRecorderManager"));
            services.Configure<RecorderManagementPagingConfig>(Configuration.GetSection("PanoptoRecorderPaging"));

            //DI for interfaces
            services.AddScoped<IScheduleGenerator, ScheduleGenerator>();
            services.AddScoped<IScheduleCreationInitiator, ScheduleCreationInitiator>();
            services.AddScoped<IConflictGenerator, ConflictGenerator>();
            services.AddScoped<ISessionGenerator, SessionGenerator>();

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = @"Server=136.142.96.135;Database=Navigator;ConnectRetryCount=0;User Id=NavReadWrite;Password=l%]k.5Ev85u";
            services.AddDbContext<NavEventsContext>(options => options.UseSqlServer(connection));
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
                app.UseHsts();
            }
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
