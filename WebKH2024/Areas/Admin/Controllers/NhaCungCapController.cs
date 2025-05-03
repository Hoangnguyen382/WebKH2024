using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: Admin/NhaCungCap
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        [QuyenNhanVien(Roles = "DanhSachNCC")]
        public ActionResult Index(string TenNhaCungCap, int? SoDienThoai)
        {
            ViewBag.TenNhaCungCap = TenNhaCungCap;
            ViewBag.SoDienThoai = SoDienThoai;
            mapNhaCungCap map = new mapNhaCungCap();
            var lst = map.DanhSach(TenNhaCungCap, SoDienThoai);
            return View(lst);
        }
        public ActionResult ChiTiet(int id)
        {
            mapNhaCungCap map = new mapNhaCungCap();
            var lst = map.ChiTiet(id);
            return View(lst);
        }
        [QuyenNhanVien(Roles = "ThemMoiNCC")]
        public ActionResult ThemMoi()
        {
            NhaCungCap sp = new NhaCungCap();
            sp.Active = false;
            sp.NgayTao = DateTime.Now;
            return View(sp);
        }
        [HttpPost]
        public ActionResult ThemMoi(NhaCungCap model)
        {
            mapNhaCungCap map = new mapNhaCungCap();
            if (map.ThemMoi(model) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [QuyenNhanVien(Roles = "CapNhatNCC")]
        public ActionResult CapNhat(int id)
        {
            mapNhaCungCap map = new mapNhaCungCap();
            var model = map.ChiTiet(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult CapNhat(NhaCungCap model)
        {
            mapNhaCungCap map = new mapNhaCungCap();
            if (map.CapNhat(model) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [QuyenNhanVien(Roles = "XoaNCC")]
        public ActionResult Xoa(int id)
        {
            mapNhaCungCap map = new mapNhaCungCap();
            if (map.Xoa(id) == true)
            { return RedirectToAction("Index"); }
            else
            {
                return View();
            }

        }
    }
}