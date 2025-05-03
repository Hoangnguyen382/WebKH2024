using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapChiTietCombo
    {
        private readonly ShopQuanAoEntities db = new ShopQuanAoEntities();

        public List<ChiTietCombo> DanhSach(int idCombo)
        {
            return db.ChiTietComboes.Where(m => m.ComboID == idCombo).ToList();
        }

        public ChiTietCombo ChiTiet(int id)
        {
            return db.ChiTietComboes.Find(id);
        }

        public bool ThemMoi(ChiTietCombo model)
        {
            if (model.ComboID == 0 || model.SanPhamID == 0)
            {
                return false;
            }
            var exists = db.ChiTietComboes.Any(c => c.ComboID == model.ComboID && c.SanPhamID == model.SanPhamID);
            if (exists)
            {
                return false;
            }
            if (model.SoLuong == null)
            {
                model.SoLuong = 0;
            }
            if (model.MauSac == null)
            {
                model.MauSac = "";
            }
            if (model.Size == null)
            {
                model.Size = "";
            }
            db.ChiTietComboes.Add(model);
            db.SaveChanges();
            return true;
        }

        public bool CapNhat(ChiTietCombo model)
        {
            var chiTietCombo = db.ChiTietComboes.Find(model.ChiTietComboID);
            if (chiTietCombo == null)
            {
                return false;
            }
            chiTietCombo.SanPhamID = model.SanPhamID;
            chiTietCombo.SoLuong = model.SoLuong;
            chiTietCombo.MauSac = model.MauSac;
            chiTietCombo.Size = model.Size;
            db.SaveChanges();
            return true;
        }

        public bool Xoa(int id)
        {
            var chiTietCombo = db.ChiTietComboes.Find(id);
            if (chiTietCombo == null)
            {
                return false;
            }

            db.ChiTietComboes.Remove(chiTietCombo);
            db.SaveChanges();
            return true;
        }
    }
}
