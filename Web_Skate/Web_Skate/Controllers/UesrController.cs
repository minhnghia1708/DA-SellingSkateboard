using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_Skate.Models;

namespace Web_Skate.Controllers
{
    public class UesrController : Controller
    {
        DBSkateDataContext data = new DBSkateDataContext();
        // GET: Uesr
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(FormCollection Collection,KhachHang KH)
        {
            var hoten = Collection["HoTen_KH"];
            var user = Collection["Account_KH"];
            var pass = Collection["Password_KH"];
            var diachi = Collection["DiaChi_KH"];
            var SDT = Collection["SDT_KH"];
            var check_username = data.KhachHangs.FirstOrDefault(n => n.Account_KH == user);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Lỗi1"] = "HỌ TÊN KHÔNG ĐƯỢC ĐỂ TRỐNG!";
            }else if(String.IsNullOrEmpty(user))
            {
                ViewData["Lỗi2"] = "USER KHÔNG ĐƯỢC ĐỂ TRỐNG!!";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["Lỗi3"] = "PASS KHÔNG ĐƯỢC ĐỂ TRỐNG!!";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Lỗi4"] = "ĐỊA CHỈ KHÔNG ĐƯỢC ĐỂ TRỐNG!!";
            }
            else if (String.IsNullOrEmpty(SDT))
            {
                ViewData["Lỗi5"] = "SĐT KHÔNG ĐƯỢC ĐỂ TRỐNG!!";
            }
            else if (check_username != null)
            {
                ViewBag.Warning = "Username này đã tồn tại!!";
            }
            else
            {
                KH.HoTen_KH = hoten;
                KH.Account_KH= user;
                KH.Password_KH= pass;
                KH.DiaChi_KH = diachi;
                KH.SDT_KH= SDT;
                data.KhachHangs.InsertOnSubmit(KH);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.CreateAccount();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login(FormCollection Collection)
        {
            var account = Collection["ACcount_KH"];
            var pass = Collection["Password_KH"];
            if (String.IsNullOrEmpty(account))
            {
                ViewData["Lôi1"] = "Phải Nhập Tên Đăng Nhập!";

            }else if(String.IsNullOrEmpty(pass))
            {
                ViewData["Lôi2"] = "Phải Nhập Mật Khẩu!";
            }
            else
            {
                KhachHang kh = data.KhachHangs.SingleOrDefault(k => k.Account_KH == account && k.Password_KH==pass);
                if (kh != null)
                {
                    
                    Session["Account_KH"]=kh;
                    return RedirectToAction("Index", "Home");
                    
                }
                else
                {
                    ViewBag.Thongbao = "Mật Khẩu Hoặc Tài Khoản Không Đúng!!";
                }    
            }
            return View();
        }
       
        public ActionResult CaNhan()
        {
            KhachHang kh = new KhachHang();
            if (Session["Account_KH"] == null)
            {
                return RedirectToAction("Login", "Uesr");
            }
            else
            {
                kh = (KhachHang)Session["Account_KH"];
            }
            return View(kh);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
             KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.ID_KH == id);
            ViewBag.ID_KH = kh.ID_KH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }            
            return View(kh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit(int id, FormCollection collection)
        {
            //Tạo 1 biến khachhang với đối tương id = id truyền vào
            var khachhang = data.KhachHangs.First(n => n.ID_KH == id);
            var hoten = collection["HoTen_KH"];
            var sdt = collection["SDT_KH"];
            var diachi = collection["DiaChi_KH"];
            var taikhoan = collection["Account_KH"];
            var pass = collection["Password_KH"];
            khachhang.ID_KH = id;
            
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Chưa nhập họ tên!";
            }
            if (string.IsNullOrEmpty(sdt))
            {
                ViewData["Loi2"] = "Chưa nhập số điện thoại!";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi3"] = "Chưa nhập địa chỉ!";
            }
            if (string.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi4"] = "Chưa nhập tài khoản!";
            }
            if (string.IsNullOrEmpty(pass))
            {
                ViewData["Loi4"] = "Chưa nhập PassWord!";
            }
            else
            {
                khachhang.HoTen_KH = hoten;
                khachhang.SDT_KH = sdt;
                khachhang.DiaChi_KH = diachi;
                khachhang.Account_KH = taikhoan;
                khachhang.Password_KH = pass;
                //Update trong CSDL
                UpdateModel(khachhang);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Edit(id);
        }
        public ActionResult DonHangCaNhan()
        {
            KhachHang kh = new KhachHang();
            if (Session["Account_KH"] != null)
                kh = (KhachHang)Session["Account_KH"];
            else
                return RedirectToAction("Login", "Uesr");

            var dsPDP = data.DonHangs.Where(t => t.ID_KH == kh.ID_KH).ToList();
            return View(dsPDP);
        }
        public ActionResult CTDonHang(int id)
        {

            CT_DonHang ct = data.CT_DonHangs.SingleOrDefault(n => n.ID_DonHang == id);
            return View(ct);
        }
        
    }
}
