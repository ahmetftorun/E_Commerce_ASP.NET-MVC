using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AhmetProj.Controllers
{
    public class HomeController : Controller
    {
        E_Commercemain_vtEntities db=new E_Commercemain_vtEntities();
        public ActionResult index()
        {
            Liste Homelist = new Liste()
            {
                sliders = db.Sliders.ToList(),
                categorys = db.Categorys.ToList(),
                products = db.Products.ToList()
            };

            return View(Homelist);
        }
       
        public ActionResult sosyalmedya() 
        {
            var sosyalmedya = db.SocialMedia.ToList();
            return PartialView(sosyalmedya);
        }

        public ActionResult category_list()
        {
            var categoryList = db.Categorys.ToList();
            return View(categoryList);
        }

        public ActionResult opportunity_item()
        {
            return View();
        }
    }
}