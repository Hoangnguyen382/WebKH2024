using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(WebKH2024.Startup))]

namespace WebKH2024
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure cookie-based authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
                CookieManager = new Microsoft.Owin.Host.SystemWeb.SystemWebCookieManager() // Make sure this is set
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            // Configure Google external authentication
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "936746707664-3j3fqu3oehmluis3mbdo1nmlgeqolqhi.apps.googleusercontent.com",
                ClientSecret = "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",
                CallbackPath = new PathString("/signin-google")
            });
        }
    }
}
