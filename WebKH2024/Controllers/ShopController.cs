using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.Models;

namespace WebKH2024.Controllers
{
    public class ShopController : Controller
    {
            ShopQuanAoEntities db = new ShopQuanAoEntities();
        // GET: Product
            public ActionResult Index(int? DanhMucID, string sortOrder,bool? sort,string TenSanPham)
            {
                ViewBag.DanhMucID = DanhMucID;
                ViewBag.SortOrder = sortOrder;
                ViewBag.TenSanPham = TenSanPham;
                mapSanPham map = new mapSanPham();
                if (sortOrder == "giatang")
                {
                    sort = true;
                }
                else if (sortOrder == "giagiam")
                {
                    sort = false;
                }

                var lstsp = map.SanPhamDM(DanhMucID, sort, TenSanPham);
                ViewBag.DanhMucs = map.LayDanhMuc();

                return View(lstsp);
            }

        public ActionResult ProductDetail(int id,bool? sort, string TenSanPham)
        {
            mapSanPham map = new mapSanPham();
            mapReview maprv = new mapReview();
            var sp = map.ChiTiet(id);
            var relatedProducts = map.SanPhamDM(sp.DanhMucID, sort, TenSanPham)
                            .Where(p => p.SanPhamID != id)
                            .ToList();
            ViewBag.RelatedProducts = relatedProducts;
            var reviews = maprv.GetReviews(id);
            ViewBag.Reviews = reviews;
            return View(sp);
        }
        [HttpPost]
		public ActionResult AddReview(int SanPhamID, string Comment, int Rating)
		{
			if (Session["User"] == null)
			{
				return RedirectToAction("Login", "Home");
			}
			if (Rating < 1 || Rating > 5)
			{
				TempData["ErrorMessage"] = "Rating must be between 1 and 5.";
				return RedirectToAction("ProductDetail", new { id = SanPhamID });
			}

            var userId = (int)Session["User"];
            var user = db.TaiKhoans.Find(userId); ;
			Review review = new Review
			{
				SanPhamID = SanPhamID,
				TaiKhoanID = user.TaiKhoanID,
				Rating = Rating,
				Comment = Comment,
				ReviewDate = DateTime.Now
			};
			mapReview map = new mapReview();
			if (map.ThemMoi(review))
			{
				return RedirectToAction("ProductDetail", new { id = SanPhamID });
			}
			else
			{
				return View();
			}
		}
    }
}