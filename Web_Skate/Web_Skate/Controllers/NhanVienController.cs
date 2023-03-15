using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Skate.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Web_Skate.Controllers
{
    public class NhanVienController : Controller
    {
        DBSkateDataContext db = new DBSkateDataContext();
        // GET: NhanVien
        public ActionResult Index()
        {
            NhanVien nv = new NhanVien();
            if (Session["NhanVien"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else  
            {
                nv = (NhanVien)Session["NhanVien"];
            }
            return View(nv);
        }
        public ActionResult SanPham(int ? page)
        {
            int PageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SanPhams.ToList());
            return View(db.SanPhams.ToList().OrderBy(n=>n.ID_SanPham).ToPagedList(PageNumber, pageSize));
        }
        public ActionResult NhanVien(int? page)
        {
            
            int PageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SanPhams.ToList());
            return View(db.NhanViens.ToList().OrderBy(n => n.ID_NV).ToPagedList(PageNumber, pageSize));
        }
        public ActionResult KhachHang(int? page)
        {

            int PageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SanPhams.ToList());
            return View(db.KhachHangs.ToList().OrderBy(n => n.ID_KH).ToPagedList(PageNumber, pageSize));
        }
        public ActionResult DonHang(int? page)
        {

            int PageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SanPhams.ToList());
            return View(db.DonHangs.ToList().OrderBy(n => n.ID_DonHang).ToPagedList(PageNumber, pageSize));
        }
        public ActionResult CTDonHang(int id)
        {

            CT_DonHang ct = db.CT_DonHangs.SingleOrDefault(n => n.ID_DonHang == id);
            return View(ct);
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            ViewBag.ID_Loai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "ID_Loai", "TenLoai");
            ViewBag.ID_ThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenThuongHieu), "ID_ThuongHieu", "TenThuongHieu");
            return View();
        }
        [HttpPost]
        public ActionResult CreateNew(SanPham SP,HttpPostedFileBase fileupload)
        {
            ViewBag.ID_Loai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "ID_Loai", "TenLoai");
            ViewBag.ID_ThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenThuongHieu), "ID_ThuongHieu", "TenThuongHieu");
            
            if(fileupload == null)
            {
                ViewBag.Thongbao = "Vui Lòng Chọn Ảnh ";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/asset/shop/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình Ảnh Đã Tồn Tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    SP.Image = fileName;
                    db.SanPhams.InsertOnSubmit(SP);
                    db.SubmitChanges();
                }
            }
            
           
            return RedirectToAction("SanPham");
        }

        public ActionResult ChiTietSanPham(int id)
        {

            var deck = from s in db.SanPhams
                       where s.ID_SanPham == id
                       select s;
            return View(deck.Single());

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var deck = from s in db.SanPhams
                       where s.ID_SanPham == id
                       select s;
            return View(deck.Single());

        }
        [HttpPost,ActionName("Delete")]
        public ActionResult XacNhanDelete(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.ID_SanPham == id);
            ViewBag.ID_SanPham=sp.ID_SanPham;
            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");

        }
       
        [HttpGet]
        public ActionResult EditMoiSP(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(a => a.ID_SanPham == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.ID_Loai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "ID_Loai", "TenLoai");
            ViewBag.ID_ThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenThuongHieu), "ID_ThuongHieu", "TenThuongHieu");
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditMoiSP(int id, HttpPostedFileBase fileUpload)
        {

           var sp = db.SanPhams.FirstOrDefault(a => a.ID_SanPham == id);
            sp.ID_SanPham = id;
            ViewBag.ID_Loai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "ID_Loai", "TenLoai");
            ViewBag.ID_ThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.TenThuongHieu), "ID_ThuongHieu", "TenThuongHieu");            
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View(sp);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/asset/shop/"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sp.Image = fileName;
                    sp.ID_SanPham = id;
                    
                    UpdateModel(sp);
                    db.SubmitChanges();
                    return RedirectToAction("SanPham");
                }
                return this.EditMoiSP(id);
            }

        }
        [HttpGet]
        public ActionResult EditDH(int id)
        {
            DonHang DH = db.DonHangs.SingleOrDefault(n => n.ID_DonHang == id);
            ViewBag.ID_DonHang = DH.ID_DonHang;
            if (DH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.ThanhToan = new SelectList(db.PhuongThucThanhToans.ToList().OrderBy(n => n.PhuongThuc), "PT_ThanhToan", "PhuongThuc");
            ViewBag.TrangThai = new SelectList(db.TrangThaiDonHangs.ToList().OrderBy(n => n.TrangThaiDH), "TrangThai", "TrangThaiDH");
            return View(DH);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditDH(DonHang dh,int id, FormCollection collection)
        {
            //Tạo 1 biến khachhang với đối tương id = id truyền vào
             dh = db.DonHangs.First(n => n.ID_DonHang == id);
          
            var kh = collection["ID_KH"];
            var ghichu = collection["GhiChu"];
            var diachi = collection["DiaChiNguoiNhan"];
            var hoten = collection["TenNguoiNhan"];
            var sdt = collection["SDT_NguoiNhan"];
            var NgayDat = collection["NgayDat"];
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
           
            dh.ID_DonHang = id;

            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Chưa Chọn Trạng Thái!";
            }
            else
            {
                dh.TenNguoiNhan = hoten;
                dh.SDT_NguoiNhan = sdt;
                dh.DiaChiNguoiNhan = diachi;
                dh.GhiChu = ghichu;
                dh.NgayDat = DateTime.Parse(NgayDat);
                dh.NgayGiao = DateTime.Parse(NgayGiao);
                ViewBag.ThanhToan = new SelectList(db.PhuongThucThanhToans.ToList().OrderBy(n => n.PhuongThuc), "PT_ThanhToan", "PhuongThuc");
            ViewBag.TrangThai = new SelectList(db.TrangThaiDonHangs.ToList().OrderBy(n => n.TrangThaiDH), "TrangThai", "TrangThaiDH");
                //Update trong CSDL
                UpdateModel(dh);
                db.SubmitChanges();
                return RedirectToAction("DonHang");
            }
            return this.EditDH(id);
        }
        public ActionResult DoanhThu()
        {
            return View();
        }
    }
}