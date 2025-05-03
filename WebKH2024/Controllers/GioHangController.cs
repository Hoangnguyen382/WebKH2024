using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebKH2024.Models;
using WebKH2024.Models.Momo;

namespace WebKH2024.Controllers
{
    public class GioHangController : Controller
    {
        private ShopQuanAoEntities db = new ShopQuanAoEntities();
        public ActionResult ShoppingCart()
        {
            var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            ViewBag.TotalPrice = cart.Sum(item => item.ThanhTien);
            ViewBag.ShippingFee = 30000;
            ViewBag.TotalAmount = ViewBag.TotalPrice + ViewBag.ShippingFee;

            return View(cart);
        }
        public ActionResult AddToCart(int productId, int? quantity, string Size, string MauSac)
        {
            var product = db.SanPhams.Find(productId);
            if (product != null)
            {
                var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
                var cartItem = cart.FirstOrDefault(i => i.SanPhamID == productId && i.Size == Size && i.MauSac == MauSac);

                if (cartItem == null)
                {
                    cart.Add(new ChiTietDonHang
                    {
                        SanPhamID = product.SanPhamID,
                        SoLuong = quantity ?? 1,
                        TenSanPham = product.TenSanPham,
                        Size = Size,
                        MauSac = MauSac,
                        GiaBan = product.GiaBan,
                        HinhAnh = product.HinhAnh,
                        PhiShip = 30000,
                        ThanhTien = (product.GiaBan * (quantity ?? 1)) ,
                    });
                }
                else
                {
                    cartItem.SoLuong += quantity ?? 1;
                    cartItem.ThanhTien = cartItem.GiaBan * cartItem.SoLuong;
                }
                Session["Cart"] = cart;
                Session["CartCount"] = cart.Count;
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AddComboToCart(int comboId)
        { 
            var combo = db.ComboKhuyenMais.Find(comboId);
            if (combo == null)
            {
                return HttpNotFound();
            }

            var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            var cartDetails = Session["CartDetails"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            var comboShippingFee = 30000;
            var existingCombo = cart.FirstOrDefault(i => i.ComboID == comboId);
            if (existingCombo == null)
            {
                cart.Add(new ChiTietDonHang
                {
                    ComboID = comboId,
                    TenSanPham = combo.TenCombo,
                    SoLuong = 1,
                    MauSac = null,
                    Size = null,
                    GiaBan = combo.GiaCombo,
                    HinhAnh = combo.HinhAnh,
                    PhiShip = comboShippingFee,
                    ThanhTien = combo.GiaCombo,
                });
                // Thêm từng sản phẩm trong combo vào giỏ hàng với Size và Màu sắc
                var chiTietCombo = db.ChiTietComboes.Where(c => c.ComboID == comboId).ToList();
                foreach (var item in chiTietCombo)
                {
                    string size = Request.Form["Size_" + item.SanPhamID]?.ToString().Trim();
                    string mauSac = Request.Form["MauSac_" + item.SanPhamID]?.ToString().Trim();


                    cartDetails.Add(new ChiTietDonHang
                    {
                        ComboID = comboId,
                        SanPhamID = item.SanPham.SanPhamID,
                        TenSanPham = item.SanPham.TenSanPham,
                        SoLuong = item.SoLuong,
                        GiaBan = item.SanPham.GiaBan,
                        HinhAnh = item.SanPham.HinhAnh,
                        Size = size,
                        MauSac = mauSac,
                        PhiShip = 0, // Không tính phí ship cho từng sản phẩm riêng lẻ trong combo
                        ThanhTien = item.SanPham.GiaBan * item.SoLuong
                    });
                }
            }
            else
            {
                existingCombo.SoLuong += 1;
                existingCombo.ThanhTien = existingCombo.GiaBan * existingCombo.SoLuong;
            }
            Session["Cart"] = cart;
            Session["CartDetails"] = cartDetails;
            Session["CartCount"] = cart.Count;
            return RedirectToAction("ShoppingCart");
        }

        public ActionResult CapNhatSoLuong(int productId, int soLuong, string size, string mauSac)
        {
            var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            var cartItem = cart.FirstOrDefault(i =>
                    (i.SanPhamID == productId && i.Size == size && i.MauSac == mauSac) ||  
                    (i.ComboID == productId) 
                );
            if (cartItem != null)
            {
                cartItem.SoLuong = soLuong;
                cartItem.ThanhTien = cartItem.GiaBan * cartItem.SoLuong;
                Session["Cart"] = cart;
                return Json(new { success = true, soLuong = cartItem.SoLuong, thanhTien = cartItem.ThanhTien }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XoaKhoiGio(int SanPhamID)
        {
            var cart = Session["Cart"] as List<ChiTietDonHang>;
            var itemXoa = cart?.FirstOrDefault(m => m.SanPhamID == SanPhamID);

            if (itemXoa != null)
            {
                cart.Remove(itemXoa);
                Session["Cart"] = cart;
                Session["CartCount"] = cart.Count;
            }
            return RedirectToAction("ShoppingCart");
        }

        
    }
}
