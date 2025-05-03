using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class ComBoKhuyenMaiController : Controller
    {
         mapComboKhuyenMai map = new mapComboKhuyenMai();
        #region CRUD Combo Khuyến Mãi
        [QuyenNhanVien(Roles = "DanhSachCombo")]
        public ActionResult Index()
        {
            ViewBag.Map = map;
            return View(map.DanhSach());
        }
        public ActionResult ChiTiet(int id)
        {
            var combo = map.ChiTiet(id);
            return View(combo);
        }
        [QuyenNhanVien(Roles = "ThemMoiCombo")]
        public ActionResult ThemMoi()
        {
           
            return View(new ComboKhuyenMai());
        }

        [HttpPost]
        public ActionResult ThemMoi(ComboKhuyenMai combo)
        {
            if (map.ThemMoi(combo))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(combo);
            }
        }
        [QuyenNhanVien(Roles = "CapNhatCombo")]
        public ActionResult CapNhat(int id)
        {
            var combo = map.ChiTiet(id);
            return View(combo);
        }

        [HttpPost]
        public ActionResult CapNhat(ComboKhuyenMai combo)
        {
            if (map.CapNhat(combo))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(combo);
            }
        }
        [QuyenNhanVien(Roles = "XoaCombo")]
        public ActionResult Xoa(int id)
        {
            map.Xoa(id);
            return RedirectToAction("Index");
        }
        public ActionResult CapNhatAnh(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult CapNhatAnh(int id, HttpPostedFileBase FileAnh)
        {
            if (FileAnh == null || FileAnh.ContentLength == 0)
            {
                ViewBag.id = id;
                ViewBag.error = "Chưa Chọn File Hoặc File Ảnh Bị Lỗi";
                return View();
            }
            var tuongdoi = "/Data/Image/"; 
            var tuyetdoi = Server.MapPath(tuongdoi);  
            
            FileAnh.SaveAs(tuyetdoi + FileAnh.FileName);
            // Lưu vào db 
            mapHinhAnh map = new mapHinhAnh();
            map.HinhAnhCombo(id, tuongdoi + FileAnh.FileName);

            return RedirectToAction("ChiTiet", new { id = id });
        }
        #endregion

        public ActionResult ThemChitietCombo(int id)
        {
            var db = new ShopQuanAoEntities();
            var danhSachSanPham = db.SanPhams
                        .Where(sp => !db.ChiTietComboes
                                        .Where(c => c.ComboID == id)
                                        .Select(c => c.SanPhamID)
                                        .Contains(sp.SanPhamID))
                                        .ToList();

            ViewBag.DanhSachSanPham = new SelectList(danhSachSanPham, "SanPhamID", "TenSanPham");
            return View(new ChiTietCombo() { ComboID = id});
        }

        [HttpPost]
        public ActionResult ThemChiTietCombo(ChiTietCombo model)
        {
            var db = new ShopQuanAoEntities();
            var map = new mapChiTietCombo();
            if (map.ThemMoi(model) == true)
            {
                return RedirectToAction("ChiTiet", new { id = model.ComboID });
            }
            else
            {
                ViewBag.DanhSachSanPham = new SelectList(
                             db.SanPhams.Where(sp => !db.ChiTietComboes
                            .Where(c => c.ComboID == model.ComboID)
                            .Select(c => c.SanPhamID)
                            .Contains(sp.SanPhamID)),
                            "SanPhamID", "TenSanPham");
                ModelState.AddModelError("", "Lỗi Thêm Mới SP");
                return View(model);
            }
        }
        public ActionResult CapNhatChiTiet(int id)
        {
            var db = new ShopQuanAoEntities();
            var danhSachSanPham = db.SanPhams
                        .Where(sp => !db.ChiTietComboes
                                        .Where(c => c.ComboID == id)
                                        .Select(c => c.SanPhamID)
                                        .Contains(sp.SanPhamID))
                                        .ToList();
            ViewBag.DanhSachSanPham = new SelectList(danhSachSanPham, "SanPhamID", "TenSanPham");
            return View(new mapChiTietCombo().ChiTiet(id));
        }
        [HttpPost]
        public ActionResult CapNhatChiTiet(ChiTietCombo model)
        {
            var db = new ShopQuanAoEntities();
            var map = new mapChiTietCombo();
            if (map.CapNhat(model) == true)
            {
                return RedirectToAction("ChiTiet", new { id = model.ComboID });
            }
            else
            {
                ViewBag.DanhSachSanPham = new SelectList(
                             db.SanPhams.Where(sp => !db.ChiTietComboes
                            .Where(c => c.ComboID == model.ComboID)
                            .Select(c => c.SanPhamID)
                            .Contains(sp.SanPhamID)),
                            "SanPhamID", "TenSanPham");
                ModelState.AddModelError("", "Lỗi Thêm Mới SP");
                return View(model);
            }
        }
        public ActionResult XoaChiTiet(int idChiTiet, int ComboID)
        {
            var map = new mapChiTietCombo();
            map.Xoa(idChiTiet);
            return RedirectToAction("ChiTiet", new { id = ComboID });
        }
    }
}