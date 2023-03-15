using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Skate.Models;

namespace Web_Skate.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        DBSkateDataContext data = new DBSkateDataContext();

        public List<Cart> Laygiohang()
        {
            List<Cart> list = Session["Cart"] as List<Cart>;
            if(list == null)
            {
                list = new List<Cart>();
                Session["Cart"]=list;
            }
            return list;
        }
        public ActionResult AddCart(int IDSP,string strURL)
        {
            List<Cart> list = Laygiohang();
            Cart sp = list.Find(n=>n.ID_SanPham == IDSP);
            if(sp == null)
            {
                sp=new Cart(IDSP);
                list.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.SoLuong++;
                return Redirect(strURL); 
            }
        }

        private int TongSoLuong()
        {
            int TongSoLuongs = 0;
            List<Cart> list = Session["Cart"] as List<Cart> ;
            if (list != null)
            {
                TongSoLuongs = list.Sum(n => n.SoLuong);
            }
            return TongSoLuongs;
        }
        private double TongTien()
        {
            double TongTiens = 0;
            List<Cart> list = Session["Cart"] as List<Cart>;
            if(list != null)
            {
                TongTiens = list.Sum(n=>n.TongTien);
            }
            return TongTiens;
        }
        public ActionResult Cart()
        {
            List<Cart> list = Laygiohang();
            if(list.Count ==0)
            {
                return RedirectToAction("Index","Shop");
            }
            ViewBag.TongSoLuong =TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(list);
        }
        public ActionResult CartPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult DeleteCart(int IDSP)
        {
            List<Cart> list = Laygiohang();
            Cart cart = list.SingleOrDefault(n => n.ID_SanPham == IDSP );
            if(cart != null)
            {
                list.RemoveAll(n => n.ID_SanPham == IDSP);
                return RedirectToAction("Cart");
            }    
            if(list.Count == 0)
            {
                return RedirectToAction("Index","Shop");

            }
            return RedirectToAction("Cart");
        }
        public ActionResult UpdateCart (int IDSP, FormCollection f)
        {
            List<Cart> list = Laygiohang();
            Cart cart = list.SingleOrDefault (n => n.ID_SanPham == IDSP);
            if(cart != null)
            {
                cart.SoLuong = int.Parse(f["quantity"].ToString());

            }
            return RedirectToAction("Cart");
        }
        public ActionResult DeleteFullCart()
        {
            List<Cart> list = Laygiohang();
            list.Clear();
            return RedirectToAction("Index", "Shop");
        }
        [HttpGet]
        public ActionResult Oder()
        {
            if (Session["Account_KH"] == null || Session["Account_KH"].ToString()=="")
            {
                return RedirectToAction("Login", "Uesr");

            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Shop");
            }
            ViewBag.PT_ThanhToan = new SelectList(data.PhuongThucThanhToans.ToList().OrderBy(n => n.PhuongThuc), "PT_ThanhToan", "PhuongThuc");
            
            List<Cart> list = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(list);
        }
        public ActionResult Oder(DonHang dh,FormCollection collection)
        {
           
            KhachHang kh = (KhachHang)Session["Account_KH"];
           
            ViewBag.PT_ThanhToan = new SelectList(data.PhuongThucThanhToans.ToList().OrderBy(n => n.PhuongThuc), "PT_ThanhToan", "PhuongThuc");
            dh.ID_KH = kh.ID_KH;
            dh.NgayDat = DateTime.Now;
            dh.TranThai = 1;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.NgayGiao=DateTime.Parse(NgayGiao);
            dh.TenNguoiNhan = kh.HoTen_KH;
            dh.SDT_NguoiNhan = kh.SDT_KH;
            dh.DiaChiNguoiNhan = kh.DiaChi_KH;
            List<Cart> list = Laygiohang();
            ViewBag.TongTien = TongTien();
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach(var item in list)
            {
                CT_DonHang ct = new CT_DonHang();
                ct.ID_DonHang = dh.ID_DonHang;
                ct.ID_SanPham = item.ID_SanPham;
                ct.SoLuong=item.SoLuong;
                ct.DonGia = (long)(decimal)item.Gia_SanPham;
               
                data.CT_DonHangs.InsertOnSubmit(ct);

            }
            data.SubmitChanges();
            Session["Cart"] = null;
           
                return RedirectToAction("Orderconfirmation", "Cart");
          
        }
        public ActionResult Orderconfirmation()
        {
            return View();
        }
       
    }
}