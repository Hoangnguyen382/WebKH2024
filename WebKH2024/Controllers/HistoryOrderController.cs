using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.Models;

namespace WebKH2024.Controllers
{
    public class HistoryOrderController : Controller
    {   
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public ActionResult Index(int id )
        {
            if (id == 0)
            {
                return RedirectToAction("Login", "Home");
            }
            var orderHistory = db.DonHangs
                .Where(dh => dh.TaiKhoanID == id)
                .OrderByDescending(dh => dh.NgayDatHang)
                .ToList();
            return View(orderHistory);
        }
        public ActionResult ChiTiet(int id)
        {
            mapDonHang map = new mapDonHang();
            var donhang = map.ChiTiet(id);
            return View(donhang);
        }
    }
}