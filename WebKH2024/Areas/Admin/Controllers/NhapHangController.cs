using System.Web.Mvc;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class NhapHangController : Controller
    {
        private ShopQuanAoEntities db = new ShopQuanAoEntities();
        private mapNhapHang map = new mapNhapHang();
        #region CRUD NHẬP HÀNG
        [QuyenNhanVien(Roles = "DanhSachNhapHang")]
        public ActionResult Index()
        {
            ViewBag.Map = map;
            return View(map.DanhSach());
        }
        public ActionResult ChiTiet(int id)
        {
            var nhapHang = map.ChiTiet(id);
            return View(nhapHang);
        }
        [QuyenNhanVien(Roles = "ThemMoiNhapHang")]
        public ActionResult ThemMoi()
        {
            NhapHang sp = new NhapHang();
            return View(sp);
        }
        [HttpPost]
        public ActionResult ThemMoi(NhapHang sp)
        {
            if (map.ThemMoi(sp) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(sp);
            }
        }
        [QuyenNhanVien(Roles = "CapNhatNhapHang")]
        public ActionResult CapNhat(int id)
        {
            var sp = map.ChiTiet(id);
            return View(sp);
        }
        [HttpPost]
        public ActionResult CapNhat(NhapHang sp)
        {
            if (map.CapNhat(sp) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(sp);
            }
        }
        [QuyenNhanVien(Roles = "XoaNhapHang")]
        public ActionResult Xoa(int id)
        {
            mapNhapHang map = new mapNhapHang();
            map.Xoa(id);
            return RedirectToAction("Index");
        }
        #endregion
        #region CRUD Chi Tiết Nhập Hàng
        public ActionResult ThemChiTiet(int id)
        {
            return View(new ChiTietNhapHang() { NhapHangID = id });
        }
        [HttpPost]
        public ActionResult ThemChiTiet(ChiTietNhapHang model)
        {
            var map = new mapChiTietNhapHang();
            if (map.ThemMoi(model) > 0)
            {
                return RedirectToAction("ChiTiet", new { id = model.NhapHangID });
            }
            else
            {
                ModelState.AddModelError("", "Lỗi Thêm Mới SP");
                return View(model);
            }
        }
        public ActionResult CapNhatChiTiet(int id)
        {
            return View(new mapChiTietNhapHang().ChiTiet(id));
        }
        [HttpPost]
        public ActionResult CapNhatChiTiet(ChiTietNhapHang model)
        {
            var map = new mapChiTietNhapHang();
            if (map.CapNhat(model) == true)
            {
                return RedirectToAction("ChiTiet", new { id = model.NhapHangID });
            }
            else
            {
                ModelState.AddModelError("", "Lỗi Thêm Mới SP");
                return View(model);
            }
        }
        public ActionResult XoaChiTiet(int idChiTiet, int NhapHangID)
        {
            var map = new mapChiTietNhapHang();
            map.Xoa(idChiTiet);
            return RedirectToAction("ChiTiet", new { id = NhapHangID });
        }
        #endregion
    }
}
