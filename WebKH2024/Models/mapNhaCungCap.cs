using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace WebKH2024.Models
{
    public class mapNhaCungCap
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public List<NhaCungCap> DanhSach(string TenNhaCungCap, int? SoDienThoai)
        {
            TenNhaCungCap = (TenNhaCungCap ?? "").ToLower();
            return db.NhaCungCaps.Where(m =>
                    (m.TenNhaCungCap.ToLower().Contains(TenNhaCungCap))
                    &&
                    (SoDienThoai == null || m.SoDienThoai == SoDienThoai )
                ).ToList(); 
        }
        public bool ThemMoi(NhaCungCap ncc)
        {
            if (string.IsNullOrEmpty(ncc.TenNhaCungCap) == true) { return false; }
            db.NhaCungCaps.Add(ncc);
            db.SaveChanges();
            return true;
        }
        public NhaCungCap ChiTiet(int id)
        {
            return db.NhaCungCaps.Find(id);
        }
        public bool CapNhat(NhaCungCap ncc)
        {
            var updateNCC = db.NhaCungCaps.Find(ncc.NhaCungCapID);
            if (updateNCC == null)
            {
                return false;
            }
            updateNCC.TenNhaCungCap = ncc.TenNhaCungCap;
            updateNCC.DiaChi = ncc.DiaChi;
            updateNCC.SoDienThoai = ncc.SoDienThoai;
            updateNCC.Email = ncc.Email;
            updateNCC.SoTaiKhoan = ncc.SoTaiKhoan;
            updateNCC.NganHang = ncc.NganHang;
            updateNCC.NgayTao = ncc.NgayTao;
            updateNCC.Active = ncc.Active;
            db.SaveChanges();
            return true;
        }
        public bool Xoa(int id)
        {
            var updateModel = db.NhaCungCaps.Find(id);
            if (updateModel == null)
            {
                return false;
            }
            db.NhaCungCaps.Remove(updateModel);
            db.SaveChanges();
            return true;
        }
    }
}