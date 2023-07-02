using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AhmetProj.Controllers
{
    public class OrdersController : Controller
    {

       E_Commercemain_vtEntities db = new E_Commercemain_vtEntities();
        public ActionResult Orders()
        {
            if (Session["user_id"] != null)
            {
                var mycart = (Cart)Session["sepetimiz"];
                var order_no = db.Orders.Max(x => x.order_no);
                order_no = order_no + 1;

                foreach (var cart_registration in mycart.Sepetim)
                {
                    Orders orders = new Orders
                    {
                        order_no = order_no,
                        user_id = ((Users)Session["user_id"]).user_id,
                        piece = cart_registration.adet,
                        order_date = DateTime.Now,
                        product_id = cart_registration.urun.product_id
                    };

                    db.Orders.Add(orders);
                    db.SaveChanges();
                }

                string siparis_msj = "İşlem Başarılı Sipariş Numaranız=" + order_no;
                return RedirectToAction("satin_al", "Cart", new { siparis_msj });
            }
            else
            {
                return RedirectToAction("sign_up", "UserLogin");
            }
        }
    }
}