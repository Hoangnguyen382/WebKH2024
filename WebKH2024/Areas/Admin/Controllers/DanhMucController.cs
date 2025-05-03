using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{

    public class DanhMucController : Controller
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        mapDanhMuc map = new mapDanhMuc();
        // GET: Admin/DanhMuc
        [QuyenNhanVien(Roles = "DanhSachDanhMuc")]
        public ActionResult Index()
        {
            var lst = map.DanhSach();
            return View(lst);
        }
        [QuyenNhanVien(Roles = "ThemMoiDanhMuc")]
        public ActionResult ThemMoi()
        {
            DanhMuc dmuc = new DanhMuc();
            return View(dmuc);
        }
        [HttpPost]
        public ActionResult ThemMoi(DanhMuc dmuc)
        {
            if (map.ThemMoi(dmuc) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(dmuc);
            }
        }
        [QuyenNhanVien(Roles = "CapNhatDanhMuc")]
        public ActionResult CapNhat(int id)
        {
            var model = map.ChiTiet(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult CapNhat(DanhMuc model)
        {
            if (map.CapNhat(model) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [QuyenNhanVien(Roles = "XoaDanhMuc")]
        public ActionResult Xoa(int id)
        {
            if (map.Xoa(id) == true)
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