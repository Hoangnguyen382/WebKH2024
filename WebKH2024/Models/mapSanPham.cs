using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapSanPham
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();
        public IPagedList<SanPham> DanhSach(string TenSanPham, int? DanhMucID, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            TenSanPham = (TenSanPham ?? "").ToLower();
            var query = db.SanPhams.Where(m =>
                    (m.TenSanPham.ToLower().Contains(TenSanPham))
                    &&
                    (DanhMucID == null || m.DanhMucID == DanhMucID)
                );
            if (startDate != null)
            {
                query = query.Where(m => m.NgayTao >= startDate);
            }
            if (endDate != null)
            {
                query = query.Where(m => m.NgayTao <= endDate);
            }

            return query.OrderBy(m => m.TenSanPham).ToPagedList(pageNumber, pageSize);
        }
        public List<SanPham> SanPhamDM(int? DanhMucID, bool? sort, string TenSanPham)
        {
            TenSanPham = (TenSanPham ?? "").ToLower();
            var query = db.SanPhams.AsQueryable();

            if (DanhMucID != null)
            {
                query = query.Where(m => m.DanhMucID == DanhMucID);
            }

            if (!string.IsNullOrEmpty(TenSanPham))
            {
                query = query.Where(m => m.TenSanPham.ToLower().Contains(TenSanPham));
            }

            if (sort == true)
            {
                query = query.OrderBy(m => m.GiaBan);
            }
            else if (sort == false)
            {
                query = query.OrderByDescending(m => m.GiaBan);
            }

            return query.ToList();
        }

        public List<SanPham> LaySP()
        {
            return db.SanPhams.ToList();   
        }

        public List<DanhMuc> LayDanhMuc()
        {
            return db.DanhMucs.ToList();
        }

        public SanPham ChiTiet(int id )
        {
            return db.SanPhams.Find( id );
        }
        public bool ThemMoi(SanPham sp)
        {
            if(string.IsNullOrEmpty(sp.TenSanPham) == true ) { return false; }
            if (sp.GiaNiemYet == null)
            {
                sp.GiaNiemYet = 0;
            }
            sp.GiaBan = sp.GiaNiemYet - (sp.GiaNiemYet) * sp.KhuyenMai/100;
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return true;
        }
        public bool CapNhat(SanPham sp)
        {
            var updateSP = db.SanPhams.Find(sp.SanPhamID);
            if (updateSP == null)
            {
                return false;
            }
            updateSP.TenSanPham = sp.TenSanPham;
            updateSP.MoTa = sp.MoTa;
            sp.GiaBan = sp.GiaNiemYet - (sp.GiaNiemYet) * sp.KhuyenMai / 100;
            updateSP.GiaNiemYet = sp.GiaNiemYet;
            updateSP.DanhMucID = sp.DanhMucID;
            updateSP.HinhAnh = sp.HinhAnh;
            updateSP.Size = sp.Size;
            updateSP.MauSac = sp.MauSac;
            updateSP.KhuyenMai = sp.KhuyenMai;
            updateSP.TrangThai= sp.TrangThai;
            updateSP.NgayTao = sp.NgayTao;
            db.SaveChanges();
            return true;
        }
        public bool Xoa(int id)
        {
            var deleteSP = db.SanPhams.Find(id);
            if (deleteSP == null)
            {
                return false;
            }
            db.SanPhams.Remove(deleteSP);
            db.SaveChanges();
            return true;
        }

    }
}