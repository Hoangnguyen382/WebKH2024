//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebKH2024.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
    
        public int DonHangID { get; set; }
        public Nullable<int> TaiKhoanID { get; set; }
        public string NguoiNhan { get; set; }
        public Nullable<System.DateTime> NgayDatHang { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoai { get; set; }
        public Nullable<int> TongTien { get; set; }
        public string GhiChu { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
    
        public virtual TaiKhoan TaiKhoan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
