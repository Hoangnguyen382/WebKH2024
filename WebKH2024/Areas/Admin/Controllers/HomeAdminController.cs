using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        // GET: Admin/HomeAdmin
        public ActionResult TrangChu()
        {
            var orderpaid = DonHangPaid();
            var ordersuccess = DonHangSuccess();
            ViewBag.orderpaid = orderpaid;
            ViewBag.ordersuccess = ordersuccess;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        // Login 
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Tên đăng nhập và mật khẩu không được để trống.";
                return View();
            }
            var admin = db.TaiKhoans.FirstOrDefault(m => m.Username == username && m.Password == password);
            if (admin == null || admin.Role == "User")
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng hoặc bạn không có đủ quyền truy cập.";
                return View();
            }
            Session["Admin"] = admin;
            return RedirectToAction("TrangChu");
        }
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult KhongCoQuyen()
        {
            return View();
        }
        public int DonHangPaid()
        {
            int soluong = db.DonHangs.Count(m => m.TrangThai == 1);
            return soluong;
        }
        public int DonHangSuccess()
        {
            int? soluong = db.DonHangs.Count(m => m.TrangThai == 3);
            return soluong ?? 0;
        }
    }
}