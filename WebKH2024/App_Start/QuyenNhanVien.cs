using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebKH2024.Models;

namespace WebKH2024.App_Start
{
    public class QuyenNhanVien : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["Admin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "HomeAdmin",
                    action = "Login",
                    area = "Admin"
                }));
                return;
            }
            var user = HttpContext.Current.Session["Admin"] as TaiKhoan;
            int ID = user.TaiKhoanID;
            string codeCN = this.Roles;
            ShopQuanAoEntities db = new ShopQuanAoEntities();
            var demPhanQuyen = db.PhanQuyenChucNangs.Count(m =>
                    m.CodeChucNang == codeCN
                    &
                    m.ID == ID
                );
            if (demPhanQuyen == 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "HomeAdmin",
                    action = "KhongCoQuyen",
                    area = "Admin"
                }));
                return;
            }
        }
    }
}