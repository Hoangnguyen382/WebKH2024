using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;

namespace WebKH2024.Models
{
    public class mapDanhMuc
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public List<DanhMuc> DanhSach()
        {
            return db.DanhMucs.ToList();
        }
        public DanhMuc ChiTiet(int id) 
        { 
            return db.DanhMucs.Find(id);   
        }
        public bool ThemMoi(DanhMuc dmuc)
        {
            if (string.IsNullOrEmpty(dmuc.TenDanhMuc) == true) { return false; }
            db.DanhMucs.Add(dmuc);
            db.SaveChanges();
            return true;
        }
        public bool CapNhat(DanhMuc dmuc)
        {
            var updateDMuc = db.DanhMucs.Find(dmuc.DanhMucID);
            if (updateDMuc == null)
            {
                return false;
            }

            updateDMuc.TenDanhMuc = dmuc.TenDanhMuc;
            updateDMuc.MoTa = dmuc.MoTa;
            updateDMuc.NgayTao = dmuc.NgayTao;
            db.SaveChanges();
            return true;
        }
        public bool Xoa(int id)
        {
            var dmuc = db.DanhMucs.Find(id);
            if (dmuc == null)
            {
                return false;
            }
            db.DanhMucs.Remove(dmuc);
            db.SaveChanges();
            return true;
        }
    }
}