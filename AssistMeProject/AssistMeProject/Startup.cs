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
using Microsoft.EntityFrameworkCore;
using AssistMeProject.Models;

namespace AssistMeProject
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
            //Setting up for loggin
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);//You can set Time   
            });
            services.AddMvc();
            //End of setting up for logging
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; //Change true if you want to show warning of cookies
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<AssistMeProjectContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AssistMeProjectContext")));

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Questions}/{action=Index}/{id?}");
            });

            addData(app.ApplicationServices);

        }

        private void addData(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.CreateScope())
            {
                var ctx = serviceScope.ServiceProvider.GetService<AssistMeProjectContext>();
                if (ctx.User.Any())
                {
                    return;   // La base de datos ya tiene datos               
                }

                var users = new List<User> {
                new User
                    {
                        EMAIL = "lauhincapie97@gmail.com", PHOTO="pic", QUESTIONS_ANSWERED = 2, POSITIVE_VOTES_RECEIVED = 3,
                        QUESTIONS_ASKED = 4, INTERESTING_VOTES_RECEIVED =5, DESCRIPTION ="Hola", COUNTRY="Colombia", CITY="Cali"
                    },
                    new User
                    {
                        EMAIL = "lauhincapie97@gmail.com", PHOTO="pic", QUESTIONS_ANSWERED = 2, POSITIVE_VOTES_RECEIVED = 3,
                        QUESTIONS_ASKED = 4, INTERESTING_VOTES_RECEIVED =5, DESCRIPTION ="Hola", COUNTRY="Colombia", CITY="Cali"
                    },
                    new User
                    {
                        EMAIL = "lauhincapie97@gmail.com", PHOTO="pic", QUESTIONS_ANSWERED = 2, POSITIVE_VOTES_RECEIVED = 3,
                        QUESTIONS_ASKED = 4, INTERESTING_VOTES_RECEIVED =5, DESCRIPTION ="Hola", COUNTRY="Colombia", CITY="Cali"
                    }};
                users.ForEach(e => ctx.User.Add(e));
                ctx.SaveChanges();
            }
        }

    }
}

