using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Skate.Models
{
    public class Cart
    {
        DBSkateDataContext data = new DBSkateDataContext();
        public int ID_SanPham { get; set; }
        public string TenSanPham { get; set; }
        public string Image { get; set; }
        public Double Gia_SanPham { set; get; }
        public int SoLuong { get; set; }
        public Double TongTien
        {
            get { return SoLuong * Gia_SanPham;  }


        }
        public Cart (int IDSP)
        {
            ID_SanPham = IDSP;
            SanPham SP = data.SanPhams.Single(k=>k.ID_SanPham == IDSP);
            TenSanPham = SP.TenSanPham;
            Image = SP.Image;
            Gia_SanPham=double.Parse(SP.Gia_SanPham.ToString());
            SoLuong = 1;

        }
    }
}