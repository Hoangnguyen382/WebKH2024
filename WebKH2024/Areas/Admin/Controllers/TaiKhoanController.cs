using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKH2024.App_Start;
using WebKH2024.Models;

namespace WebKH2024.Areas.Admin.Controllers
{
    public class TaiKhoanController : Controller
    {
        public ActionResult Index()
        {
            return View(new mapTaiKhoan().DanhSach());
        }
        public ActionResult ChiTiet(int TaiKhoanID)
        {
         
            return View(new mapTaiKhoan().ChiTiet(TaiKhoanID));
        }
        public ActionResult ThemMoi()
        {   
            TaiKhoan tk = new TaiKhoan();
            tk.NgayTao = DateTime.Now;
            return View(tk);
        }
        [HttpPost]
        public ActionResult ThemMoi(TaiKhoan tk)
        {
            mapTaiKhoan map = new mapTaiKhoan();
            if (map.ThemMoi(tk) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(tk);
            }
        }
        public ActionResult CapNhat(int id)
        {
            return View(new mapTaiKhoan().ChiTiet(id));
        }
        [HttpPost]
        public ActionResult CapNhat(TaiKhoan tk)
        {
            mapTaiKhoan map = new mapTaiKhoan();
            if (map.CapNhat(tk) == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(tk);
            }
        }
        public ActionResult Xoa(int id)
        {
            mapTaiKhoan map = new mapTaiKhoan();
            if (map.Xoa(id) == true)
            { 
                return RedirectToAction("Index"); 
            }
            else
            {
                return View();
            }
        }
        public ActionResult LuuPhanQuyen(int TaiKhoanID, string ChucNang, bool? check)
        {
            var map = new mapPhanQuyenChucNang();
            map.LuuPhanQuyen(TaiKhoanID, ChucNang, check);
            return RedirectToAction("ChiTiet", new {TaiKhoanID = TaiKhoanID});
        }
    }
}