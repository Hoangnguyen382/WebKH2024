using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(API.App_Start.Startup))]
namespace API.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Bật CORS(chia sẻ tài nguyên gốc chéo) để thực hiện yêu cầu bằng trình duyệt từ các miền khác nhau
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                //Tạo đường dẫn url lấy token
                TokenEndpointPath = new PathString("/token"),
                //Đặt thời gian hết hạn của token
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(10),
                //Khai báo class sẽ sử dụng để xác thực thông tin người dùng
                Provider = new CheckUserLogin()
            };
            //Tạo token
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
