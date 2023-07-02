using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AhmetProj.Controllers
{
    public class CartController : Controller
    {
        E_Commercemain_vtEntities db = new E_Commercemain_vtEntities();
        public Cart Sepeti_getir()
        {
            var sepetimiz = (Cart)Session["sepetimiz"];
            //session içerisinde oluşrurduğumuz sepetimiz nesnemizin adresi var
            if (sepetimiz == null)//1 kez
            {
                sepetimiz = new Cart();
                Session["sepetimiz"] = sepetimiz;
            }
            return sepetimiz;
        }

        // GET: Sepet
        public ActionResult Sepetim()
        {
            if (Session["sepetimiz"] != null)
            {
                Cart sepet = Sepeti_getir();
                List<Carts> sepetListesi = new List<Carts>();

                foreach (var item in sepet.Sepetim)
                {
                    sepetListesi.Add(new Carts { urun = item.urun, adet = item.adet });
                }

                Liste model = new Liste();
                model.carts = sepetListesi;
                return View(model);
            }
           
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Sepete_ekle(int? id, byte? adet)//linklede geliyor adet kutusu formatında gelecek
        {
            var _adet = adet ?? 0;
            var urun = db.Products.FirstOrDefault(x => x.product_id == id);
            if (urun != null)
            {
                Sepeti_getir().Sepete_ekle(urun, _adet);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Sepetim");
        }

        public ActionResult satin_al(string siparis_msj, Users users)
        {
            // Kullanıcının oturum açmış olup olmadığını kontrol et
            if ((Users)Session["user_id"] != null)
            {
                ViewBag.siparis_msj = siparis_msj;

              
                    var id = ((Users)Session["user_id"]).user_id;
                    var addresses = db.UsersAdress.Where(x => x.user_id == id).ToList();
                    ViewBag.Addresses = addresses;
                

                return View();
            }
            else
            {
                return RedirectToAction("sign_in", "UserLogin");
            }
        }

        public ActionResult sepetten_sil(int? id)
        {
            var urun = db.Products.FirstOrDefault(x => x.product_id == id);
            if (urun != null)
            {
                Sepeti_getir().Sepetten_sil(urun);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Sepetim");
        }
     
    }
}