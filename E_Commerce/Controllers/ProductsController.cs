using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;

namespace AhmetProj.Controllers
{
    public class ProductsController : Controller
    {
        E_Commercemain_vtEntities db = new E_Commercemain_vtEntities();
        // GET: Products
        public ActionResult product_list()
        {
            var brands=db.Brands.ToList();
            var product=db.Products.ToList();
            Liste liste = new Liste();

         liste.brands = brands;
            liste.products= product;
            ViewBag.brand_id = new SelectList(db.Brands,"brand_id","brand_name");
          return View(liste);
        }


        [HttpPost]
        public ActionResult product_list(int? brand_id)
        {
            //Markaya gore listeleme yapıyoruz
            Liste liste = new Liste();
            if (brand_id == null)
            {
                var product = db.Products.ToList();
                liste.products = product;   
            }
            else
            {
                var brand = db.Products.Where(x => x.brand_id == brand_id).ToList();
                liste.products = brand;
            }
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name");
            return View(liste);
        }

       

        public ActionResult product_detail(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Burada, veritabanından verilen id değerine sahip ürünü aldık. Eğer bu ürün bulunamazsa, istenen kaynağın bulunamadığını belirten HttpNotFound sonucunu döner.
            var product = db.Products.FirstOrDefault(x => x.product_id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            //Bu kısım, Liste turunde bir nesne olusturduk ve products listesine product turunde ekliyorz. ve view a model turundeki listemizi ekliyoruz.
            var model = new Liste
            {
                products = new List<Products> { product }
            };

            return View(model);
        }
    }


}
