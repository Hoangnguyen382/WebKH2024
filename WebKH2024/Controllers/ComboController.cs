using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.Models;

namespace WebKH2024.Controllers
{
    public class ComboController : Controller
    {
        mapComboKhuyenMai map = new mapComboKhuyenMai();
        // GET: Combo
        public ActionResult Index()
        {
            return View(map.DanhSach());
        }
        public ActionResult ChiTiet(int id)
        {
            var combo = map.ChiTiet(id);
            return View(combo);
        }
    }
}