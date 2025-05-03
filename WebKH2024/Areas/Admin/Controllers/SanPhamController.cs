using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        // GET: Admin/SanPham
        #region CRUD Sản Phẩm
        [QuyenNhanVien(Roles = "DanhSachSanPham")]
        public ActionResult Index(string TenSanPham, int? DanhMucID, DateTime? startDate, DateTime? endDate, int? page)
        {
            ViewBag.TenSanPham = TenSanPham;
            ViewBag.DanhMucID = DanhMucID;
            ViewBag.startDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.endDate = endDate?.ToString("yyyy-MM-dd");
            int pageSize = 10;
            int pageNumber = (page ?? 1); 
            mapSanPham map = new mapSanPham();
            var lstsp = map.DanhSach(TenSanPham, DanhMucID, startDate, endDate, pageNumber, pageSize);
            return View(lstsp);
        }
        [HttpGet]
        public JsonResult TimKiemSanPham(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Json(new { success = false, message = "Từ khóa trống" }, JsonRequestBehavior.AllowGet);
            }

            var sanPhams = db.SanPhams
                .Where(s => s.TenSanPham.Contains(keyword))
                .Select(s => new
                {
                    s.SanPhamID,
                    s.TenSanPham,
                    s.HinhAnh,
                    GiaBan = s.GiaBan 
                })
                .Take(10)
                .ToList();
            return Json(sanPhams, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChiTiet(int id)
        {
            mapSanPham map = new mapSanPham();
            var lst = map.ChiTiet(id);
            return View(lst);
        }

        [QuyenNhanVien(Roles = "ThemMoiSanPham")]

        public ActionResult ThemMoi()
        {
            SanPham sp = new SanPham();
            sp.GiaNiemYet = 30000;
            sp.SoLuong = 50;
            sp.TrangThai = false;
            sp.NgayTao = DateTime.Now;
            return View(sp);
        }
        [HttpPost]
        public ActionResult ThemMoi(SanPham sp)
        {
            mapSanPham map = new mapSanPham();
            if (map.ThemMoi(sp) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(sp);
            }
        }
        [QuyenNhanVien(Roles = "CapNhatSanPham")]
        public ActionResult CapNhat(int id)
        {
            mapSanPham map = new mapSanPham();
            var sp = map.ChiTiet(id);
            return View(sp);
        }
        [HttpPost]
        public ActionResult CapNhat(SanPham sp)
        {
            mapSanPham map = new mapSanPham();
            if (map.CapNhat(sp) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(sp);
            }
        }
        [QuyenNhanVien(Roles = "XoaSanPham")]
        public ActionResult Xoa(int id)
        {
            mapSanPham map = new mapSanPham();

            if (map.Xoa(id) == true)
            {
                return RedirectToAction("Index");
            }
            else
            { 
                return View(); 
            }

        }

        #endregion

        public ActionResult CapNhatAnh(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult CapNhatAnh(int id,HttpPostedFileBase FileAnh)
        {
            if (FileAnh == null || FileAnh.ContentLength == 0)
            {
                ViewBag.id = id;
                ViewBag.error = "Chưa Chọn File Hoặc File Ảnh Bị Lỗi";
                return View();
            }
            var tuongdoi = "/Data/Image/"; // Lấy đường đãn tương đối lưu db
            var tuyetdoi = Server.MapPath(tuongdoi); // Lấy đường dẫn tuyệt đối lưu trên server 
            // Lưu File (Chưa check trùng tên file )
            FileAnh.SaveAs(tuyetdoi + FileAnh.FileName);
            // Lưu vào db 
            mapHinhAnh map = new mapHinhAnh();
            map.CapNhatHinhAnh(id, tuongdoi + FileAnh.FileName);

            return RedirectToAction("ChiTiet", new {id = id});
        }

    }
}