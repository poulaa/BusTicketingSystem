using bus_system3.Areas.Identity.Data;
using bus_system4.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;
using airline_reservation_system4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace bus_system4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StripeConfiguration.ApiKey = configuration.GetValue<string>("Stripe:SecretKey");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddDbContext<bus_context>(options =>
                   options.UseSqlServer(
                       Configuration.GetConnectionString("airline_contextConnection")));

            services.AddDefaultIdentity<bus_user>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<bus_context>();
            //services.Configure<stripesettings>(Configuration.GetSection("stripe"));

           services.AddMvcCore(options =>
           {
               var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
               options.Filters.Add(new AuthorizeFilter(policy));    

           }
           
           ).AddXmlSerializerFormatters();  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // StripeConfiguration.SetApiKey(Configuration.GetSection("stripe")["Secretkey"]);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();  
            });
        }
    }
}
