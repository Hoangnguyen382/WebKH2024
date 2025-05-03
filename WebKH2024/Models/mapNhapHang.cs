using System.Collections.Generic;
using System.Linq;

namespace WebKH2024.Models
{
    public class mapNhapHang
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();

        public List<NhapHang> DanhSach()
        {
            return db.NhapHangs.ToList();
        }

        public NhapHang ChiTiet(int id)
        {
            return db.NhapHangs.Find(id);
        }
        public List<TaiKhoan> ListTaiKhoan()
        {
            return db.TaiKhoans.ToList();
        }

        public List<NhaCungCap> ListNCC()
        {
            return db.NhaCungCaps.ToList();
        }
        public bool ThemMoi(NhapHang nhapHang)
        {
            if (nhapHang.NhaCungCapID == null) 
                    { return false; }
            db.NhapHangs.Add(nhapHang);
            db.SaveChanges();
            return true;
        }

        public bool CapNhat(NhapHang nhapHang)
        {
            var nh = db.NhapHangs.Find(nhapHang.NhapHangID);
            if (nh == null)
            {
                return false;
            }
            nh.NgayNhap = nhapHang.NgayNhap;
            nh.NhaCungCapID = nhapHang.NhaCungCapID;
            nh.TaiKhoanID = nhapHang.TaiKhoanID;
            nh.GhiChu = nhapHang.GhiChu;
            nh.TinhTrang = nhapHang.TinhTrang;
            nh.NgayThanhToan = nhapHang.NgayThanhToan;
            nh.ThanhToan = nhapHang.ThanhToan;
            db.SaveChanges();
            return true;
        }

        public bool Xoa(int id)
        {
            var nh = db.NhapHangs.Find(id);
            if (nh == null)
            {
                return false;
            }

            db.NhapHangs.Remove(nh);
            db.SaveChanges();
            return true;
        }
        public string TinhTrangNhapHang(int? status)
        {
            if (!status.HasValue)
            {
                return "<span class=\"badge badge-secondary\">Không Xác Định</span>";
            }

            switch (status.Value)
            {
                case 1:
                    return "<span class=\"badge badge-danger\">Chưa Nhận Hàng</span>";
                case 2:
                    return "<span class=\"badge badge-warning\">Đang Xử Lý</span>";
                case 3:
                    return "<span class=\"badge badge-success\">Đã Nhận Hàng</span>";
                default:
                    return "";
            }
        }
    }
}
