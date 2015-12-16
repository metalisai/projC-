using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LendWeb.Services;
using Model;
using DAL.Interfaces;
using DAL;
using DAL.Repositories;
using MongoDB.Driver;
using Microsoft.AspNet.Identity;
using CustomIdentity;
using BLL;
using BLL.Interfaces;

namespace LendWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(x =>
            { x.Password.RequiredLength = 1;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonLetterOrDigit = false;
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = false;
                });

            services.AddMvc();

            // Add application services.
            services.AddOptions();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IMongoDBClient, MongoDBClient>();

            // TODO: wrap these to identity project somehow
            services.AddScoped<IUserLoginStore<User>, UserStore>();
            services.AddScoped<IUserRoleStore<User>, UserStore>();
            services.AddScoped<IUserClaimStore<User>, UserStore>();
            services.AddScoped<IUserPasswordStore<User>, UserStore>();
            services.AddScoped<IUserSecurityStampStore<User>, UserStore>();
            services.AddScoped<IUserEmailStore<User>, UserStore>();
            services.AddScoped<IUserLockoutStore<User>, UserStore>();
            services.AddScoped<IUserPhoneNumberStore<User>, UserStore>();
            services.AddScoped<IUserTwoFactorStore<User>, UserStore>();
            services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<IdentityRole>, RoleStore>();

            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            services.AddScoped<ILendingService, LendingService>();
            services.AddScoped<IUsersService, UsersService>();

            services.Configure<MongoDBConnectionSettings>(Configuration.GetSection("MongoDBConnectionSettings"));
            //services.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        // TODO: what is this?
                        /*serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();*/
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
