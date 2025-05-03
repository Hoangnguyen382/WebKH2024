    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using WebKH2024.Helpers;
    using WebKH2024.Models;

    namespace WebKH2024.Controllers
    {
        public class PaymentController : Controller
        {
                ShopQuanAoEntities db = new ShopQuanAoEntities();
            // GET: Payment
            [HttpGet]
            public ActionResult Checkout()
            {
                if (Session["User"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
                ViewBag.TotalPrice = cart.Sum(m => m.ThanhTien);
                ViewBag.ShippingFee = 30000;
                ViewBag.TotalAmount = ViewBag.TotalPrice + ViewBag.ShippingFee;

                return View(cart);
            }
        [HttpPost]
        public ActionResult Checkout(string DiaChiGiaoHang, string SoDienThoai, string NguoiNhan, string GhiChu, string PaymentMethod, FormCollection form)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            var cartDetails = Session["CartDetails"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
            foreach (var item in cart)
            {
                var product = db.SanPhams.Find(item.SanPhamID);
                if (product != null && product.SoLuong < item.SoLuong)
                {
                    TempData["ErrorMessage"] = $"Sản phẩm {item.TenSanPham} không đủ số lượng để mua hàng.";
                    return RedirectToAction("ShoppingCart", "GioHang");
                }
            }
            try
            {
                var order = new DonHang
                {
                    TaiKhoanID = (int)Session["User"],
                    NguoiNhan = NguoiNhan,
                    GhiChu = GhiChu,
                    NgayDatHang = DateTime.Now,
                    DiaChiGiaoHang = DiaChiGiaoHang,
                    SoDienThoai = SoDienThoai,
                    TongTien = (int)Math.Round((double)(cart.Sum(item => item.ThanhTien)) + 30000),
                    TrangThai = 1,
                    PaymentMethod = PaymentMethod,
                    PaymentStatus = null,
                };

                db.DonHangs.Add(order);
                db.SaveChanges();

                foreach (var item in cart)
                {
                    if (item.ComboID != null)
                    {
                        var comboDetails = cartDetails.Where(c => c.ComboID == item.ComboID).ToList();
                        foreach (var detail in comboDetails)
                        {
                            var orderDetail = new ChiTietDonHang
                            {
                                DonHangID = order.DonHangID,
                                SanPhamID = detail.SanPhamID,
                                ComboID = item.ComboID,
                                TenSanPham = detail.TenSanPham,
                                SoLuong = detail.SoLuong,
                                HinhAnh = detail.HinhAnh,
                                GiaBan = detail.GiaBan,
                                Size = detail.Size,
                                MauSac = detail.MauSac,
                                PhiShip = 30000,
                                ThanhTien = detail.GiaBan * detail.SoLuong,
                            };
                            db.ChiTietDonHangs.Add(orderDetail);
                        }
                    }
                    else
                    {
                        var orderDetail = new ChiTietDonHang
                        {
                            DonHangID = order.DonHangID,
                            SanPhamID = item.SanPhamID,
                            ComboID = null,
                            TenSanPham = item.TenSanPham,
                            SoLuong = item.SoLuong,
                            HinhAnh = item.HinhAnh,
                            GiaBan = item.GiaBan,
                            Size = item.Size,
                            MauSac = item.MauSac,
                            PhiShip = item.PhiShip,
                            ThanhTien = item.ThanhTien
                        };
                        db.ChiTietDonHangs.Add(orderDetail);
                    }
                }
                db.SaveChanges();
                if (PaymentMethod == "COD")
                {
                    order.TrangThai = 1;
                    db.SaveChanges();
                    TempData["OrderStatus"] = "Success";
                    TempData["OrderMessage"] = "Đặt hàng thành công. Cảm ơn quý khách!";
                }
                else if (PaymentMethod == "Momo")
                {
                    return RedirectToAction("MoMoPayment", new
                    {
                        DiaChiGiaoHang,
                        SoDienThoai,
                        NguoiNhan,
                        GhiChu,
                        PaymentMethod = "Momo"
                    });
                }
                else
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn phương thức thanh toán!";
                    return RedirectToAction("Checkout", "Payment");
                }
                Session["Cart"] = null;
                Session["CartCount"] = 0;
                TempData["OrderStatus"] = "Success";
                TempData["OrderMessage"] = "Đặt hàng thành công. Cảm ơn quý khách!";
            }
            catch (Exception)
            {
                TempData["OrderStatus"] = "Failure";
                TempData["OrderMessage"] = "Đặt hàng không thành công. Vui lòng thử lại.";
                return RedirectToAction("ShoppingCart", "GioHang");
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult MoMoPayment(string DiaChiGiaoHang, string SoDienThoai, string NguoiNhan, string GhiChu, string PaymentMethod)
            {
                if (Session["User"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();
                if (!cart.Any())
                {
                    TempData["ErrorMessage"] = "Giỏ hàng của bạn trống. Vui lòng thêm sản phẩm.";
                    return RedirectToAction("Checkout", "Payment");
                }
                try
                {
                    var order = new DonHang
                    {
                        TaiKhoanID = (int)Session["User"],
                        NguoiNhan = NguoiNhan,
                        GhiChu = GhiChu,
                        NgayDatHang = DateTime.Now,
                        DiaChiGiaoHang = DiaChiGiaoHang,
                        SoDienThoai = SoDienThoai,
                        TongTien = (int)cart.Sum(item => item.ThanhTien),
                        TrangThai = 1,
                        PaymentMethod = "Momo"
                    };
                    db.DonHangs.Add(order);
                    db.SaveChanges();

                    foreach (var item in cart)
                    {
                        var orderDetail = new ChiTietDonHang
                        {
                            DonHangID = order.DonHangID,
                            SanPhamID = item.SanPhamID,
                            ComboID = item.ComboID,
                            TenSanPham = item.TenSanPham,
                            SoLuong = item.SoLuong,
                            HinhAnh = item.HinhAnh,
                            GiaBan = item.GiaBan,
                            Size = item.Size,
                            MauSac = item.MauSac,
                            PhiShip = item.PhiShip,
                            ThanhTien = item.ThanhTien
                        };
                        db.ChiTietDonHangs.Add(orderDetail);
                    }
                    db.SaveChanges();

                    // Tạo yêu cầu thanh toán MoMo
                    var totalAmount = (int)Math.Round((double)(cart.Sum(item => item.ThanhTien) + 30000)); 

                    var orderId = order.DonHangID.ToString();
                    var requestId = Guid.NewGuid().ToString();
                    var orderInfo = "Thanh toán đơn hàng #" + orderId;

                    var returnUrl = Url.Action("MoMoReturn", "Payment", null, Request.Url.Scheme);
                    var notifyUrl = Url.Action("MoMoNotify", "Payment", null, Request.Url.Scheme);

                    var payUrl = MoMoPaymentHelper.CreatePaymentRequest(orderId, requestId, totalAmount, orderInfo, returnUrl, notifyUrl);
                    if (payUrl != null)
                    {
                        return Redirect(payUrl);
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
                    return RedirectToAction("Checkout", "Payment");
                }

                TempData["ErrorMessage"] = "Không thể kết nối với MoMo. Vui lòng thử lại.";
                return RedirectToAction("Checkout", "Payment");
            }
            public ActionResult MoMoReturn(string orderId, string resultCode, string transId)
            {
                var order = db.DonHangs.FirstOrDefault(o => o.DonHangID.ToString() == orderId);
                if (order == null)
                {
                    TempData["OrderStatus"] = "Error";
                    TempData["OrderMessage"] = "Không tìm thấy đơn hàng. Vui lòng kiểm tra lại.";
                    return RedirectToAction("Error", "Home");
                }

                if (resultCode == "0") 
                {
                    order.TrangThai = 1; 
                    order.TransactionID = transId;
                    order.PaymentStatus = 0;
                }
                else
                {
                    order.TrangThai = 1;
                    order.PaymentStatus = 1;
                    TempData["OrderMessage"] = "Thanh toán thất bại. Vui lòng thử lại.";
                }

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }


            public ActionResult MoMoNotify()
            {
                return new HttpStatusCodeResult(200);
            }
        }
    }