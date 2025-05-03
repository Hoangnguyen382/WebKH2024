    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    namespace WebKH2024.Models
    {
        public class mapChiTietNhapHang
        {
            ShopQuanAoEntities db = new ShopQuanAoEntities();
            public List<ChiTietNhapHang> DanhSach(int idNhapHang)
            {
                return db.ChiTietNhapHangs.Where(m => m.NhapHangID == idNhapHang).ToList();
            }
            public ChiTietNhapHang ChiTiet(int id)
            {
                return db.ChiTietNhapHangs.Find(id);
            }
            public int ThemMoi(ChiTietNhapHang model)
            {
                if (model.NhapHangID == 0 || model.SanPhamID == 0)
                {
                    return 0;
                }
                var sp = db.SanPhams.Find(model.SanPhamID);
                if (sp != null)
                {
                    model.TenSanPham = sp.TenSanPham;
                    sp.SoLuong += model.SoLuong;
                }

                if (model.SoLuong == null)
                {
                    model.SoLuong = 0;
                }

                if (model.GiaNhap == null)
                {
                    model.GiaNhap = 0;
                }

                model.TongTien = model.SoLuong * model.GiaNhap;
                db.ChiTietNhapHangs.Add(model);
                db.SaveChanges();
                return model.ChiTietNhapHangID;
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
                var nh = db.ChiTietNhapHangs.Find(id);
                if (nh == null)
                {
                    return false;
                }
                var sp = db.SanPhams.Find(nh.SanPhamID);
                if (sp != null)
                {
                    sp.SoLuong -= nh.SoLuong; 
                }
                db.ChiTietNhapHangs.Remove(nh);
                db.SaveChanges();
                return true;
            }

        }
    }