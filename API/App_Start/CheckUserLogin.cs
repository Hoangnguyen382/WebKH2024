using Microsoft.Owin.Security.OAuth; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API.App_Start
{
    public class CheckUserLogin : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (string.IsNullOrEmpty(context.UserName) == true)
            {
                context.SetError("Lỗi đăng nhập", "Tài khoản hoặc mật khẩu không đúng");
                return;
            } 
            if (context.UserName != "admin")
            { 
                context.SetError("Lỗi đăng nhập", "Tài khoản không tồn tại!");
                return;
            }
            //Kiểm tra trạng thái
            if (context.Password != "123456")
            {
                context.SetError("Lỗi đăng nhập", "Mật khẩu không đúng");
                return; 
            }
           
            //đăng nhập thành công
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            context.Validated(identity);
        }
    }
}
