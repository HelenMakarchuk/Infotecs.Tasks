using IdentityServer4.Models;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddCors();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(
                    new List<Client>
                    {
                        new Client
                        {
                            ClientId = "angular",
                            ClientName = "Angular Client",
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AllowedGrantTypes = GrantTypes.Code,
                            RequirePkce = true,
                            RequireClientSecret = false,
                            AllowedScopes = new List<string> {"openid", "profile", "api"},
                            RedirectUris = new List<string> { $"{Configuration["Clients:angular:Url"]}/authentication/login-callback" },
                            PostLogoutRedirectUris = new List<string> { $"{Configuration["Clients:angular:Url"]}/authentication/logout-callback" },
                            AllowedCorsOrigins = new List<string> { Configuration["Clients:angular:Url"] },
                            AllowAccessTokensViaBrowser = true
                        }
                    }
                )
                .AddAspNetIdentity<ApplicationUser>()

                // not recommended for production - you need to store your key material somewhere secure
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "530152864147-1a8tl9g4qn7q9pcn1dcve81vcnhqls2g.apps.googleusercontent.com";
                    options.ClientSecret = "HfCgET10TGky_Zup600tc34D";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration["Clients:angular:Url"])
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
