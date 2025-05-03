using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebKH2024.Models;

namespace WebKH2024.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        // GET: Home
        public ActionResult Index(int? DanhMucID, bool? sort, string TenSanPham)
        {
            double? ShippingFee = 0;
            ViewBag.DanhMucID = DanhMucID;
            mapSanPham map = new mapSanPham();
            var lstsp = map.SanPhamDM(DanhMucID, sort, TenSanPham);
            ViewBag.DanhMucs = map.LayDanhMuc();
            var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            
            foreach (var item in cart)
            {
                item.ThanhTien = item.GiaBan * item.SoLuong;
            }
            ShippingFee = cart.Count > 0 ? 30000 : 0;
            ViewBag.CartTotal = cart.Sum(item => item.SoLuong * item.GiaBan) + ShippingFee;
            ViewBag.Cart = cart;
            return View(lstsp);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Tên đăng nhập và mật khẩu không được để trống.";
                return View();
            }

            var user = db.TaiKhoans.FirstOrDefault(m => m.Username == username && m.Password == password);
            if (user != null)
            {
                Session["User"] = user.TaiKhoanID;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TaiKhoan tk, string confirm_password)
        {
            if (string.IsNullOrWhiteSpace(tk.Username) || string.IsNullOrWhiteSpace(tk.Password) || string.IsNullOrWhiteSpace(confirm_password))
            {
                TempData["Error"] = "Vui lòng điền vào tất cả thông tin bắt buộc.";
                return View();
            }
            if (!IsValidPassword(tk.Password))
            {
                TempData["Error"] = "Mật khẩu phải chứa ít nhất một chữ cái, một số và có độ dài trên 6 ký tự.";
                return View();
            }
            if (tk.Password != confirm_password)
            {
                TempData["Error"] = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View();
            }
            var checktk = db.TaiKhoans.FirstOrDefault(m => m.Username == tk.Username);
            if (checktk != null)
            {
                TempData["Error"] = "Username đã có rồi. Vui lòng chọn một username mới.";
                return View();
            }

            tk.NgayTao = DateTime.Now;
            tk.Role = "User";
            db.TaiKhoans.Add(tk);
            db.SaveChanges();
            TempData["Success"] = "Đăng ký thành công! Hãy đăng nhập.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }
        
        public ActionResult ChangePassword()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword, string ConfirmNewPassword)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }
            int userId = (int)Session["User"];
            var user = db.TaiKhoans.FirstOrDefault(u => u.TaiKhoanID == userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Người dùng không tồn tại.";
                return View();
            }
            if (user.Password != CurrentPassword)
            {
                ViewBag.ErrorMessage = "Mật khẩu hiện tại không đúng.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(NewPassword) || !IsValidPassword(NewPassword))
            {
                ViewBag.ErrorMessage = "Mật khẩu mới không hợp lệ. Mật khẩu phải chứa ít nhất một chữ cái, một số và có độ dài trên 6 ký tự.";
                return View();
            }
            if (NewPassword != ConfirmNewPassword)
            {
                ViewBag.ErrorMessage = "Xác nhận mật khẩu mới không khớp.";
                return View();
            }
            user.Password = NewPassword;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private bool IsValidPassword(string password)
        {
            // Yêu cầu mật khẩu
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[a-z]+");
            var hasMinimum6Chars = new Regex(@".{6,}");

            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum6Chars.IsMatch(password);
        }
        #region Login With Google
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }
        
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);
            var identity = result.Identity;

            if (identity != null)
            {
                var userName = identity.FindFirst(ClaimTypes.Name).Value;
                var email = identity.FindFirst(ClaimTypes.Email)?.Value;

                var user = db.TaiKhoans.FirstOrDefault(m => m.Username == userName);
                if (user == null)
                {
                    user = new TaiKhoan
                    {
                        Username = userName,
                        Email = email,
                        NgayTao = DateTime.Now,
                        Role = "User"
                    };
                    db.TaiKhoans.Add(user);
                    db.SaveChanges();
                }
                Session["User"] = user.TaiKhoanID;
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login");
        }
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["User"] = null;
            return RedirectToAction("Index");
        }
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}