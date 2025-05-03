using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapTaiKhoan
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public List<TaiKhoan> DanhSach()
        {
            return db.TaiKhoans.Where(tk => tk.Role == "NhanVien").ToList();
        }
        public TaiKhoan ChiTiet(int TaiKhoanID)
        {
            return db.TaiKhoans.Find(TaiKhoanID);
        }
        public bool ThemMoi(TaiKhoan tk)
        {
            if (tk.TaiKhoanID == null)
            {
                return false;
            }
            db.TaiKhoans.Add(tk);
            db.SaveChanges();
            return true;
        }
        public bool CapNhat(TaiKhoan tk)
        {
            var updateTK = db.TaiKhoans.Find(tk.TaiKhoanID);
            if (updateTK == null)
            {
                return false;
            }
            updateTK.HoTen = tk.HoTen;
            updateTK.Username = tk.Username;
            updateTK.Password = tk.Password;
            updateTK.Email = tk.Email;
            updateTK.SoDienThoai = tk.SoDienThoai;
            updateTK.DiaChi = tk.DiaChi;
            updateTK.Role = tk.Role;
            db.SaveChanges();
            return true;
        }
        public bool Xoa(int id)
        {
            var updateModel = db.TaiKhoans.Find(id);
            if (updateModel == null)
            {
                return false;
            }
            db.TaiKhoans.Remove(updateModel);
            db.SaveChanges();
            return true;
        }
    }
}