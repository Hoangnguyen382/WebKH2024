using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models
{
    public class mapReview
    {
        ShopQuanAoEntities db = new ShopQuanAoEntities();

        public List<Review> DanhSach()
        {
            return db.Reviews.ToList();
        }
        public List<Review> GetReviews(int sanPhamId)
        {
            return db.Reviews.Where(r => r.SanPhamID == sanPhamId)
                             .OrderByDescending(r => r.ReviewDate)
                             .ToList();
        }

        public Review ChiTiet(int id)
        {
            return db.Reviews.Find(id);
        }

        public bool ThemMoi(Review rv)
        {
            if (rv.Rating < 1 || rv.Rating > 5)
            {
                return false;
            }
            db.Reviews.Add(rv);
            db.SaveChanges();
            return true;
        }
        public bool CapNhat(Review rv)
        {
            var updateReview = db.Reviews.Find(rv.ReviewID);
            if (updateReview == null)
            {
                return false;
            }
            updateReview.SanPhamID = rv.SanPhamID;
            updateReview.TaiKhoanID = rv.TaiKhoanID;
            updateReview.Rating = rv.Rating;
            updateReview.Comment = rv.Comment;
            updateReview.ReviewDate = rv.ReviewDate;
            db.SaveChanges();
            return true;
        }

        public bool Xoa(int id)
        {
            var deleteReview = db.Reviews.Find(id);
            if (deleteReview == null)
            {
                return false;
            }

            db.Reviews.Remove(deleteReview);
            db.SaveChanges();
            return true;
        }
    }
}