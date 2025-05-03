using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace WebThucPham.App_Start
{
    public class CheckToken : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //Lấy thông tin user: Name => owin tự phân giải token để lấy thông tin
            var userName = HttpContext.Current.User.Identity.Name;

            //
            if (string.IsNullOrEmpty(userName))
            {
                var errorResponse = new { error = "Token không hợp lệ" };
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.Unauthorized,
                    errorResponse,
                    actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                );
            }
            //Check quyền:  this.Roles 
            return;
        }
    }
}
