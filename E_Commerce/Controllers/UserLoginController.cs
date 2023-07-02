using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace AhmetProj.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        E_Commercemain_vtEntities db = new E_Commercemain_vtEntities();
        // GET: Users
        public ActionResult Index()
        {

            return View();
        }
        /***************************************************************************/
        /*KULLANICI KAYIT OLMA İŞLEMLERİ*/
        public ActionResult sign_up()
        {

            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> sign_up(Users users)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Users.Any(x => x.user_email == users.user_email))
                    {
                        ModelState.AddModelError("", "Bu Email zaten kullanılmıs.");
                        return View(users);
                    }

                    // SAH256 ya gore hashlıyoruz
                    var passwordHash = EncryptPassword(users.user_password);


                    //hashlenmiş parola ile yeni bir Kullanıcı nesnesi oluşturuyoruz
                    var usera = new Users
                    {
                        user_name = users.user_name,
                        user_surname = users.user_surname,
                        user_phonenumber = users.user_phonenumber,
                        user_username = users.user_username,
                        user_membershipdate=DateTime.Now,
                        user_email = users.user_email,
                        user_password = passwordHash
                    };

                    db.Users.Add(usera);
                    db.SaveChanges();


                    return RedirectToAction("sign_in", "UserLogin");
                }
                catch (DbEntityValidationException ex)
                {

                    foreach (var error in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in error.ValidationErrors)
                        {
                            Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }



            }

            return View(users);

        }




        /***************************************************************************/

        /***************************************************************************/
        /*GİRİŞ YAP İŞLEMLERİ*/
        public ActionResult sign_in()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> sign_in(Users users)
        {

            if (ModelState.IsValid)
            {


                var user = db.Users.SingleOrDefault(x => x.user_email == users.user_email);
                if (user != null)
                {
                    // METODU KULLANARAK ŞİFREYİ KULLANICNIN ŞİFRESİ HALİNE GETİRİYOR
                    var passwordHash = EncryptPassword(users.user_password);

                    // GELEN SİFRE VERİTABANIYLA EŞELİYORMU
                    if (passwordHash == user.user_password)
                    {
                        // DOGRULAMA BAŞARILI,KULLANICIYI KAYDET
                        FormsAuthentication.SetAuthCookie(user.user_email, false);

                        //KULLANICI BİLGİLERİNİ SAKLA
                        Session["user_id"]=user;
                       

                        // KULLANICININ ROLÜNÜ BELİRLE
                        var userRole = db.Roles.FirstOrDefault(r => r.roles_id == user.roles_id)?.roles_name;
                        Session["role"] = userRole;

                        return RedirectToAction("Index", "Home");
                    }
                }
                //siteye giren kullanıcı vt de yoksa misafir olarak işaretlenir
                Session["role"] = Session["role"] ?? "guest";
            }

            return RedirectToAction("sign_in", "UserLogin");
        }
       
        public ActionResult Logout()
        {
            // Kullanıcının kimlik doğrulama çerezini geçersiz kılmak için FormsAuthentication.SignOut() yöntemi kullanılır
            FormsAuthentication.SignOut();

            // Session'daki tüm verileri temizle
            Session.Clear();
            return RedirectToAction("sign_in", "UserLogin");
        }

        // Şifreyi UTF-8 SHA256 yontemiyle şifreleyen metot
        public static string EncryptPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedBytes);

        }
    }
}