using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Skate.Models;
using PagedList;
using PagedList.Mvc;

namespace Web_Skate.Controllers
{
    public class ShopController : Controller
    {

        // GET: Shop
        DBSkateDataContext data = new DBSkateDataContext();
        private List<SanPham> laySanPhamMoi(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.Gia_SanPham).Take(count).ToList();
        }
       
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pagenum = (page ?? 1);


            var sanphammoi = laySanPhamMoi(30000);
           
            return View(sanphammoi.ToPagedList(pagenum, pageSize));
          


        }
        public ActionResult Clothes(int ? page)
        {

            int pageSize = 20;
            int pagenum = (page ?? 1);


            var sanphammoi = laySanPhamMoi(30000);

            return View(sanphammoi.ToPagedList(pagenum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var deck = from s in data.SanPhams
                       where s.ID_SanPham == id
                       select s;
            return View(deck.Single());
        }
    
       


    }
}