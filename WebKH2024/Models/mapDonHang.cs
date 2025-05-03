using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapDonHang
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();

        public IPagedList<DonHang> DanhSach(string TaiKhoan, string SoDienThoai, DateTime? NgayDat, int pageNumber, int pageSize)
        {
            TaiKhoan = (TaiKhoan ?? "").ToLower();
            var query = db.DonHangs.Where(m =>
                    (m.TaiKhoan.Username.ToLower().Contains(TaiKhoan))
                    &&
                    (string.IsNullOrEmpty(SoDienThoai) == true || m.SoDienThoai == SoDienThoai)
                );
            if(NgayDat != null)
            {
                query = query.Where(m => m.NgayDatHang >= NgayDat);
            }  
            return query.OrderByDescending(m => m.NgayDatHang).ToPagedList(pageNumber, pageSize);
        }

        public DonHang ChiTiet(int id)
        {
            return db.DonHangs.Find(id);
        }
        public List<TaiKhoan> ListTaiKhoan()
        {
            return db.TaiKhoans.ToList();
        }
        public bool CapNhat(DonHang model)
        {
            var donHang = db.DonHangs.Find(model.DonHangID);
            if (donHang == null)
            {
                return false;
            }
            if (model.TrangThai == 3 && donHang.TrangThai != 3)
            {
                foreach (var item in donHang.ChiTietDonHangs)
                {
                    var product = db.SanPhams.FirstOrDefault(p => p.SanPhamID == item.SanPhamID);
                    if (product != null)
                    {
                        if (product.SoLuong >= item.SoLuong)
                        {
                            product.SoLuong -= item.SoLuong;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            donHang.TaiKhoanID = model.TaiKhoanID;
            donHang.GhiChu = model.GhiChu;
            donHang.NguoiNhan = model.NguoiNhan;
            donHang.NgayDatHang = model.NgayDatHang;
            donHang.DiaChiGiaoHang = model.DiaChiGiaoHang;
            donHang.SoDienThoai = model.SoDienThoai;
            donHang.TongTien = model.TongTien;
            donHang.TrangThai = model.TrangThai;
            db.SaveChanges();
            return true;
        }
        public bool Xoa(int id)
        {
            var nh = db.DonHangs.Find(id);
            if (nh == null)
            {
                return false;
            }

            db.DonHangs.Remove(nh);
            db.SaveChanges();
            return true;
        }
        public string TinhTrangDonHang(int? status)
        {
            if (!status.HasValue)
            {
                return "<span class=\"badge badge-secondary\">Không Xác Định</span>";
            }
            switch (status.Value)
            {
                case 1:
                    return "<span class=\"badge badge-danger\">Đơn Hàng Mới</span>";
                case 2:
                    return "<span class=\"badge badge-warning\">Đang Xử Lý</span>";
                case 3:
                    return "<span class=\"badge badge-success\">Thành Công</span>";
                case 4:
                    return "<span class=\"badge badge-success\">Hoàn Tiền</span>";
                case 5:
                    return "<span class=\"badge badge-success\">Hủy</span>";
                default:
                    return "";
            }
        }
        public string TrangThaiThanhToan(int? status)
        {
            if (!status.HasValue)
            {
                return "<span class=\"badge badge-secondary\">Không Xác Định</span>";
            }
            switch (status.Value)
            {
                case 0:
                    return "<span class=\"badge badge-danger\">Thanh Toán Thành Công</span>";
                case 1:
                    return "<span class=\"badge badge-warning\">Thanh Toán Thất Bại</span>";
                default:
                    return "";
            }
        }
        public string TrangThaiDonHangBadge(int? trangThai)
        {
            switch (trangThai)
            {
                case 1:
                    return "<span class=\"badge badge-danger\">Đơn Hàng Mới</span>";
                case 2:
                    return "<span class=\"badge badge-warning\">Đang Xử Lý</span>";
                case 3:
                    return "<span class=\"badge badge-success\">Thành Công</span>";
                case 4:
                    return "<span class=\"badge badge-info\">Hoàn Tiền</span>";
                case 5:
                    return "<span class=\"badge badge-danger\">Hủy</span>";
                default:
                    return "<span class='badge badge-dark'>Không rõ</span>";
            }
        }

    }
}