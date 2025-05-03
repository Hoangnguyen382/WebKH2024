using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapPhanQuyenChucNang
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public List<spQuyenTaiKhoan_Result> QuyenTaiKhoan (int ID)
        {
            return db.spQuyenTaiKhoan(ID).ToList();
        }
        public void LuuPhanQuyen(int TaiKhoanID, string ChucNang, bool? check)
        {
            if(check == true)
            {
                var PhanQuyen = db.PhanQuyenChucNangs.SingleOrDefault(m => m.ID == TaiKhoanID && m.CodeChucNang == ChucNang);
                if(PhanQuyen == null)
                {
                    PhanQuyen = new PhanQuyenChucNang();
                    PhanQuyen.ID = TaiKhoanID;
                    PhanQuyen.CodeChucNang = ChucNang;
                    PhanQuyen.GhiChu = "";
                    db.PhanQuyenChucNangs.Add(PhanQuyen);
                    db.SaveChanges();   
                }
            }
            if(check != true)
            {
                var PhanQuyen = db.PhanQuyenChucNangs.SingleOrDefault(m => m.ID == TaiKhoanID && m.CodeChucNang == ChucNang);
                if(PhanQuyen != null)
                {
                    db.PhanQuyenChucNangs.Remove(PhanQuyen);
                    db.SaveChanges();
                }    
            }    
        }
    }
}