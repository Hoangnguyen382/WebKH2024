using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapHinhAnh
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public bool CapNhatHinhAnh(int id, string urlHinhAnh)
        {
            try
            {
                var img = db.SanPhams.Find(id);
                img.HinhAnh = urlHinhAnh;
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false; 
            }
        }
        public bool HinhAnhCombo(int id, string urlHinhAnh)
        {
            try
            {
                var img = db.ComboKhuyenMais.Find(id);
                img.HinhAnh = urlHinhAnh;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}