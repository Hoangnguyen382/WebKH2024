using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapComboKhuyenMai
    {
        private readonly ShopQuanAoEntities db = new ShopQuanAoEntities();

        public List<ComboKhuyenMai> DanhSach()
        {
            var combos = db.ComboKhuyenMais.ToList();
            foreach (var item in combos)
            {
                CapNhatTrangThaiCombo(item);
            }
            db.SaveChanges();
            return combos;
        }

        public ComboKhuyenMai ChiTiet(int id)
        {
            var combo = db.ComboKhuyenMais.Find(id);
            if (combo != null)
            {
                CapNhatTrangThaiCombo(combo);
                db.SaveChanges();
            }
            return combo;
        }

        public bool ThemMoi(ComboKhuyenMai combo)
        {
            if (string.IsNullOrEmpty(combo.TenCombo) == true)
            {
                return false;
            }
            CapNhatTrangThaiCombo(combo);
            db.ComboKhuyenMais.Add(combo);
            db.SaveChanges();
            return true;
        }

        public bool CapNhat(ComboKhuyenMai model)
        {
            var combo = db.ComboKhuyenMais.Find(model.ComboID);
            if (combo == null)
            {
                return false;
            }
            combo.TenCombo = model.TenCombo;
            combo.GiaCombo = model.GiaCombo;
            combo.SoLuong = model.SoLuong;
            combo.NgayBatDau = model.NgayBatDau;
            combo.NgayKetThuc = model.NgayKetThuc;
            combo.MoTa = model.MoTa;
            combo.HinhAnh = model.HinhAnh;
            CapNhatTrangThaiCombo(combo);

            db.SaveChanges();
            return true;
        }

        public bool Xoa(int id)
        {
            var combo = db.ComboKhuyenMais.Find(id);
            if (combo == null)
            {
                return false;
            }

            db.ComboKhuyenMais.Remove(combo);
            db.SaveChanges();
            return true;
        }
        private void CapNhatTrangThaiCombo(ComboKhuyenMai combo)
        {
            if ((DateTime.Now < combo.NgayBatDau || DateTime.Now > combo.NgayKetThuc))
            {
                combo.TrangThai = false;
            }
            else 
            {
                combo.TrangThai = true;
            }
        }
        public List<SanPham> LayDanhSachSanPham()
        {
            return db.SanPhams.ToList();
        }
    }
}