using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class ReviewController : Controller
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        [QuyenNhanVien(Roles = "DanhSachReview")]
        public ActionResult Index()
        {
            var danhSachSanPham = db.SanPhams.ToList();
            return View(danhSachSanPham);
        }
        public ActionResult ChiTiet(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            var reviews = db.Reviews.Where(r => r.SanPhamID == id).OrderByDescending(r => r.ReviewDate).ToList();

            ViewBag.SanPham = sanPham;
            return View(reviews);
        }
        [QuyenNhanVien(Roles = "ThemMoiReview")]
        public ActionResult ThemMoi()
        {
            return View(new Review());
        }

        [HttpPost]
        public ActionResult ThemMoi(Review review)
        {
            mapReview map = new mapReview();
            if (map.ThemMoi(review) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(review);
            }
        }
        [QuyenNhanVien(Roles = "CapNhatReview")]
        public ActionResult CapNhat(int id)
        {
            mapReview map = new mapReview();
            var review = map.ChiTiet(id);
            return View(review);
        }

        [HttpPost]
        public ActionResult CapNhat(Review review)
        {
            mapReview map = new mapReview();
            if (map.CapNhat(review))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(review);
            }
        }
        [QuyenNhanVien(Roles = "XoaReview")]
        public ActionResult Xoa(int id)
        {
            mapReview map = new mapReview();
            if (map.Xoa(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        
    }
}