using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Pluralsight.AspNetCore.Auth.Web.Services;

namespace Pluralsight.AspNetCore.Auth.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => {
                options.Filters.Add(new RequireHttpsAttribute());
            });


            services.AddAuthentication(
                options => {
                    options.DefaultChallengeScheme= CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = "Temporary";
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }
               )
              .AddFacebook(options=> {
                  options.AppId = "";
                  options.AppSecret = "";
              }
              )
              .AddTwitter(options=> {
                  options.ConsumerKey = "";
                  options.ConsumerSecret = "";
              })
              .AddGoogle(options=> {
                  options.ClientId = "";
                  options.ClientSecret = "-L";
              })
              .AddCookie(options=> {
                  options.LoginPath = "/auth/signin";
              })
              .AddCookie("Temporary");


            services.AddSingleton<IUserService, DummyUserService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRewriter(new RewriteOptions().AddRedirectToHttps(301, 44343));

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
