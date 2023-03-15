using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Skate.Models;

namespace Web_Skate.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        DBSkateDataContext db = new DBSkateDataContext();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["Account_NV"];
            var pass = collection["Password_NV"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải Nhập UserName";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["Loi2"] = "Phải Nhập PassWord";
            }
            else
            {
                NhanVien NV = db.NhanViens.SingleOrDefault(n => n.Account_NV == tendn && n.Password_NV == pass);
                if (NV != null)
                {
                    Session["NhanVien"] = NV;
                    return RedirectToAction("Index", "NhanVien");
                }
                else
                    ViewBag.ThongBao = "Wrong account or password";
            }
            return View();
        }
        [HttpGet]
        public ActionResult CreateAccount()
        {
            ViewBag.ID_Quyen = new SelectList(db.Quyen_NVs.ToList().OrderBy(n => n.Quyen), "ID_Quyen", "Quyen");
            ViewBag.ID_ChucVu = new SelectList(db.ChucVu_NVs.ToList().OrderBy(n => n.ChucVu), "ID_ChucVu", "ChucVu");

            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(NhanVien NV, FormCollection Collection)
        {



            var hoten = Collection["HoTen_NV"];
            var user = Collection["Account_NV"];
            var pass = Collection["Password_NV"];
            var email = Collection["Email_NV"];
            var SDT = Collection["SDT_NV"];
            var quyen = Collection["ID_quyen"];
            var chucvu = Collection["ID_ChucVu"];
            var check_username = db.NhanViens.FirstOrDefault(n => n.Account_NV == user);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Lỗi1"] = "HỌ TÊN KHÔNG ĐƯỢC ĐỂ TRỐNG!";
            }
            else if (String.IsNullOrEmpty(user))
            {
                ViewData["Lỗi2"] = "USER KHÔNG ĐƯỢC ĐỂ TRỐNG!!";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["Lỗi3"] = "PASS KHÔNG ĐƯỢC ĐỂ TRỐNG!!";
            }
            else if (String.IsNullOrEmpty(email))
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
                NV.HoTen_NV = hoten;
                NV.Account_NV = user;
                NV.Password_NV = pass;
                NV.Email_NV = email;
                NV.SDT_NV = SDT;
                ViewBag.ID_Loai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "ID_Loai", "TenLoai");
                ViewBag.ID_ThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenThuongHieu), "ID_ThuongHieu", "TenThuongHieu");

                db.NhanViens.InsertOnSubmit(NV);
                db.SubmitChanges();
                return RedirectToAction("SanPham");
            }
            return this.CreateAccount();
        }
    }
}