using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        private ShopQuanAoEntities db = new ShopQuanAoEntities();
        private mapDonHang map = new mapDonHang();
        #region CRUD ĐƠN HÀNG
        [QuyenNhanVien(Roles = "DanhSachDonHang")]

        public ActionResult Index(string TaiKhoan, string SoDienThoai, DateTime? NgayDat,int? page)
        {
            ViewBag.Map = map;
            ViewBag.TaiKhoan = TaiKhoan;
            ViewBag.SoDienThoai = SoDienThoai;
            ViewBag.NgayDat = NgayDat?.ToString("dd-MM-yyyy"); ;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(map.DanhSach(TaiKhoan, SoDienThoai, NgayDat, pageNumber, pageSize));
        }
        public ActionResult ChiTiet(int id)
        {
            var donhang = map.ChiTiet(id);
            return View(donhang);
        }
        [QuyenNhanVien(Roles = "CapNhatDonHang")]
        public ActionResult CapNhat(int id)
        {
            var dh = map.ChiTiet(id);
            return View(dh);
        }
        [HttpPost]
        public ActionResult CapNhat(DonHang dh)
        {
            if (map.CapNhat(dh) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(dh);
            }
        }
        [QuyenNhanVien(Roles = "XoaDonHang")]
        public ActionResult Xoa(int id)
        {
            map.Xoa(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult CapNhatTrangThai(int donHangID, int trangThaiMoi)
        {
            var donHang = db.DonHangs.Find(donHangID);
            if (donHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng." });
            }

            var trangThaiCu = donHang.TrangThai;

            // Chỉ cho phép cập nhật nếu trạng thái hợp lệ
            if (trangThaiCu == 1 && (trangThaiMoi == 2 || trangThaiMoi == 5) ||
                trangThaiCu == 2 && (trangThaiMoi == 3 || trangThaiMoi == 4 || trangThaiMoi == 5))
            {
                donHang.TrangThai = trangThaiMoi;
                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }

            return Json(new { success = false, message = "Không thể cập nhật trạng thái này." });
        }


        #endregion
        #region CRUD Chi Tiết Đơn Hàng
        public ActionResult ThemChiTiet(int id)
        {
            return View(new ChiTietDonHang() { DonHangID = id });
        }
        [HttpPost]
        public ActionResult ThemChiTiet(ChiTietDonHang model)
        {
            var map = new mapChiTietDonHang();
            if (map.ThemMoi(model) > 0)
            {
                return RedirectToAction("ChiTiet", new { id = model.DonHangID });
            }
            else
            {
                ModelState.AddModelError("", "Lỗi Thêm Mới SP");
                return View(model);
            }
        }
        public ActionResult XoaChiTiet(int idChiTiet, int DonHangID)
        {
            var map = new mapChiTietDonHang();
            map.Xoa(idChiTiet);
            return RedirectToAction("ChiTiet", new { id = DonHangID });
        }
        #endregion
    }
}