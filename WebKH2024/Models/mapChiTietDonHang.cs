using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapChiTietDonHang
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public List<ChiTietDonHang> DanhSach(int idDonHang)
        {
            return db.ChiTietDonHangs.Where(m => m.DonHangID == idDonHang).ToList();
        }
        public ChiTietDonHang ChiTiet(int id)
        {
            return db.ChiTietDonHangs.Find(id);
        }
        public int ThemMoi(ChiTietDonHang model)
        {
            if (model.DonHangID == 0 || model.SanPhamID == 0)
            {
                return 0;
            }
            var sp = db.SanPhams.Find(model.SanPhamID);
            if (sp != null)
            {
                model.MauSac = sp.MauSac;
                model.Size = sp.Size;
            }
            if (model.SoLuong == null)
            {
                model.SoLuong = 0;
            }
            if (model.ThueVat == null)
            {
                model.ThueVat = 0;
            }
            if (model.GiaBan == null)
            {
                model.GiaBan = 0;
            }
            model.ThanhTien = (model.GiaBan * model.SoLuong) + model.PhiShip + (model.GiaBan * model.SoLuong) * model.ThueVat/100;
            db.ChiTietDonHangs.Add(model);
            db.SaveChanges();
            return model.ChiTietID;
        }
        public bool CapNhat(ChiTietNhapHang model)
        {
            var update = db.ChiTietNhapHangs.Find(model.ChiTietNhapHangID);
            if (model.SanPhamID == 0)
            {
                return false;
            }
            var sp = db.SanPhams.Find(model.SanPhamID);
            if (sp != null)
            {
                update.TenSanPham = sp.TenSanPham;
                sp.SoLuong += model.SoLuong;
                sp.SoLuong -= update.SoLuong;
            }
            if (model.SoLuong == null)
            {
                update.SoLuong = 0;
            }

            if (model.GiaNhap == null)
            {
                update.GiaNhap = 0;
            }
            model.TongTien = model.SoLuong * model.GiaNhap;
            db.SaveChanges();
            return true;
        }
        public bool Xoa(int id)
        {
            var nh = db.ChiTietDonHangs.Find(id);
            if (nh == null)
            {
                return false;
            }
            var sp = db.SanPhams.Find(nh.SanPhamID);
            if (sp != null)
            {
                sp.SoLuong -= nh.SoLuong;
            }
            db.ChiTietDonHangs.Remove(nh);
            db.SaveChanges();
            return true;
        }
    }
}