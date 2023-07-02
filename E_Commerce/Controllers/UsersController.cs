using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;
using System.Net;

namespace AhmetProj.Controllers
{
    public class UsersController : Controller
    {
        E_Commercemain_vtEntities db = new E_Commercemain_vtEntities();


     
        public ActionResult myaccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           var users = db.Orders.Where(x=> x.user_id == id).ToList();
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }




        public ActionResult user_adress()
        { return View(); }
        [HttpPost]
        public ActionResult user_adress(UsersAdress usersAdress)
        {
            // Kullanıcı kimliği oturumda mevcut mu?
            if (Session["user_id"] != null)
            {
                // Oturumdaki kullanıcı kimliğini al
                var uye_id = ((Users)Session["user_id"]).user_id;

                // Kullanıcı kimliğini adres nesnesine ata
                usersAdress.user_id = uye_id;

                // Adresi veritabanına ekle
                var save = db.UsersAdress.Add(usersAdress);
                db.SaveChanges();

                // Adresin görüntüsünü döndür
                return View(usersAdress);
            }

            // Kullanıcı kimliği oturumda yoksa, giriş yapma sayfasına yönlendir
            return RedirectToAction("sign_in", "UserLogin");
        }

        public ActionResult update_adress(int? id)
        {
            // Geçerli bir id değeri var mı?
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // İlgili id'ye sahip adresi bul
            UsersAdress adress = db.UsersAdress.Find(id);

            // Adres bulunamadıysa hata döndür
            if (adress == null)
            {
                return HttpNotFound();
            }

            // Adresin düzenleme sayfasını döndür
            return View(adress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult update_adress(UsersAdress adress)
        {
            // Model geçerli mi?
            if (ModelState.IsValid)
            {
                // Adresi değiştir ve kaydet
                db.Entry(adress).State = EntityState.Modified;
                db.SaveChanges();

                // Kullanıcı adreslerine yönlendir
                return RedirectToAction("user_adress", "Users");
            }

           
            return View(adress);
        }

        public ActionResult list_adress(int id)
        {
            // Geçerli bir id değeri var mı?
            if (id != null)
            {
                // Kullanıcının adreslerini kontrol et ve listele
                var control = db.UsersAdress.Where(x => x.user_id == id).ToList();

                // Adres listesini görüntüle
                return View(control);
            }
            else
            {
                // Kullanıcı kimliği oturumda yoksa, giriş yapma sayfasına yönlendir
                return RedirectToAction("sign_in", "UserLogin");
            }
        }

        public ActionResult delete_adress(int id)
        {
            // İlgili id'ye sahip adresi bul
            UsersAdress adres = db.UsersAdress.Find(id);

            // Adresi sil ve değişiklikleri kaydet
            db.UsersAdress.Remove(adres);
            db.SaveChanges();

            // Kullanıcı adreslerine yönlendir
            return RedirectToAction("user_adress", "Users");
        }

    }
}