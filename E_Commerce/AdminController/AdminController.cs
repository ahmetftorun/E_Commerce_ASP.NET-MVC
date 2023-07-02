using AhmetProj.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AhmetProj.AdminController
{
    public class AdminController : Controller
    {
        private E_Commercemain_vtEntities db = new E_Commercemain_vtEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        /*URUN İSLEMLERİ*/
        public async Task<ActionResult> urun_listele()
        {
            //if (Session["adkuladi"] == null && Session["adkulsifre"] == null)
            //{
            //    return RedirectToAction("");/*E-ticaret sayfamızdaki girişe yonlendirilecek*/
            //}
            //else
            //{
            var products = db.Products.Include(x => x.Categorys);
            return View(products);
            //}

        }
        public ActionResult urun_ekle()
        {
            ViewBag.category_id = new SelectList(db.Categorys, "category_id", "category_name");
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> urun_ekle(Products products, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();

                if (files != null && files.Length > 0)
                {
                    int i = 1;
                    foreach (var image in files)
                    {
                        if (image != null)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var path = Path.Combine(Server.MapPath("~/urun_resimleri/"),
                                          string.Format("{0}_{1}", products.product_id, i) +
                                          Path.GetExtension(fileName));
                            image.SaveAs(path);

                            if (i == 1)
                            {
                                products.product_img1 = "/urun_resimleri/" +
                                                      string.Format("{0}_{1}",
                                                      products.product_id, i) +
                                                      Path.GetExtension(fileName);
                            }
                            else if (i == 2)
                            {
                                products.product_img2 = "/urun_resimleri/" +
                                                      string.Format("{0}_{1}",
                                                      products.product_id, i) +
                                                      Path.GetExtension(fileName);
                            }
                            else if (i == 3)
                            {
                                products.product_img3 = "/urun_resimleri/" +
                                                      string.Format("{0}_{1}",
                                                      products.product_id, i) +
                                                      Path.GetExtension(fileName);
                            }
                            i++;
                        }
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("urun_listele");
            }
            ViewBag.category_id = new SelectList(db.Categorys, "category_id", "category_name", products.category_id);
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name", products.brand_id);
            return View(products);

        }



        public ActionResult urun_guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categorys, "category_id", "category_name", products.category_id);
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name", products.brand_id);
            return View(products);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult urun_guncelle(Products products, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                if (products.product_id != null)
                {

                    db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();


                    if (files != null && files.Length > 0)
                    {
                        int i = 1;
                        foreach (var image in files)
                        {
                            if (image != null)
                            {
                                var fileName = Path.GetFileName(image.FileName);
                                var path = Path.Combine(Server.MapPath("~/urun_resimleri/"),
                                              string.Format("{0}_{1}", products.product_id, i) +
                                              Path.GetExtension(fileName));
                                image.SaveAs(path);

                                if (i == 1)
                                {
                                    products.product_img1 = "/urun_resimleri/" +
                                                          string.Format("{0}_{1}",
                                                          products.product_id, i) +
                                                          Path.GetExtension(fileName);
                                }
                                else if (i == 2)
                                {
                                    products.product_img2 = "/urun_resimleri/" +
                                                          string.Format("{0}_{1}",
                                                          products.product_id, i) +
                                                          Path.GetExtension(fileName);
                                }
                                else if (i == 3)
                                {
                                    products.product_img3 = "/urun_resimleri/" +
                                                          string.Format("{0}_{1}",
                                                          products.product_id, i) +
                                                          Path.GetExtension(fileName);
                                }
                                i++;
                            }
                        }

                        db.SaveChanges();
                    }

                }
                return RedirectToAction("urun_listele");
            }

            ViewBag.category_id = new SelectList(db.Categorys, "category_id", "category_name", products.category_id);
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name", products.brand_id);
            return View("urun_listele", products);
        }
        public async Task<ActionResult> urun_sil(int id)
        {
            Products products = await db.Products.FindAsync(id);
            db.Products.Remove(products);
            await db.SaveChangesAsync();
            return RedirectToAction("urun_listele", "Admin");
        }
        /***********************************************************************************/
        /*SİPARİS İSLEMLERİ*/
        public ActionResult siparis_gecmisi()
        {

            var siparisler = db.Orders.ToList();
            return View(siparisler);

        }

        public async Task<ActionResult> sip_sil(int? id)
        {

            Orders orders = await db.Orders.FindAsync(id);
            db.Orders.Remove(orders);
            await db.SaveChangesAsync();
            return RedirectToAction("siparis_gecmisi", "Admin");

        }
        /**************************************************************************/
        /*MARKA İŞLEMLERİ*/
        public ActionResult marka_listele()
        {
            var brands = db.Brands.ToList();
            return View(brands);
        }

        public ActionResult marka_ekle()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> marka_ekle(Brands brands)
        {
            db.Brands.Add(brands);
            await db.SaveChangesAsync();
            return View();
        }

        public async Task<ActionResult> marka_sil(int? id)
        {

            Brands brands = await db.Brands.FindAsync(id);
            db.Brands.Remove(brands);
            await db.SaveChangesAsync();
            return RedirectToAction("marka_ekle", "Admin");

        }
        /**********************************************************/
        /*KATEGORİ İSLEMLERİ*/
        public ActionResult kategori_listele()
        {
            var categorys = db.Categorys.ToList();
            return View(categorys);
        }

        public ActionResult kategori_ekle()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> kategori_ekle(Categorys categorys)
        {
            db.Categorys.Add(categorys);
            await db.SaveChangesAsync();
            return View();
        }

        public async Task<ActionResult> kategori_sil(int? id)
        {

            Categorys categorys = await db.Categorys.FindAsync(id);
            db.Categorys.Remove(categorys);
            await db.SaveChangesAsync();
            return RedirectToAction("kategori_ekle", "Admin");

        }
        /**************************************************************************/

        /*Slider İşlemleri*/
        public ActionResult slider_listesi()
        {
            var slider = db.Sliders.ToList();
            return View(slider);
        }
        public async Task<ActionResult> slider_ekle()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> slider_ekle(Sliders yeni_slider, HttpPostedFileBase file)
        {
            string dosya_adi = "resimyok.jpg";

            if (file != null)
            {
                if (file.ContentLength > 0)//dosya transfer olduysa
                {
                    String uzanti = Path.GetExtension(file.FileName);
                    if (uzanti.Equals(".jpg") || uzanti.Equals(".png"))
                    {
                        dosya_adi = Path.GetFileName(file.FileName);
                        String yol = Path.Combine(Server.MapPath("~/urun_resimleri"), dosya_adi);
                        file.SaveAs(yol);

                        ViewBag.durum = "Resim transferi oldu ve bigiler vt kaydedildi";
                    }
                    else
                    {
                        ViewBag.durum = "Resim dosyası seçmeniz gerekiyor.Yanlış tip";
                    }
                }
                else
                {
                    ViewBag.durum = " Resimsiz bigiler vt kaydedildi";
                }
            }//null

            yeni_slider.slider_img = dosya_adi;
            db.Sliders.Add(yeni_slider);
            db.SaveChanges();

            return View();
        }

        public async Task<ActionResult> slider_sil(int? id)
        {

            Sliders sliders = await db.Sliders.FindAsync(id);
            db.Sliders.Remove(sliders);
            await db.SaveChangesAsync();
            return RedirectToAction("slider_listesi", "Admin");

        }

        public ActionResult slider_guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sliders sliders = db.Sliders.Find(id);
            if (sliders == null)
            {
                return HttpNotFound();
            }

            return View(sliders);
        }


        [HttpPost]
        public async Task<ActionResult> slider_guncelle(int id, HttpPostedFileBase file, Sliders slider)
        {
            if (ModelState.IsValid)
            {
                string dosya_adi = "resimyok.jpg";

                if (file != null && file.ContentLength > 0)
                {
                    String uzanti = Path.GetExtension(file.FileName);
                    if (uzanti.Equals(".jpg") || uzanti.Equals(".png"))
                    {
                        dosya_adi = Path.GetFileName(file.FileName);
                        String yol = Path.Combine(Server.MapPath("~/urun_resimleri"), dosya_adi);
                        file.SaveAs(yol);

                        ViewBag.durum = "Resim transferi oldu ve bilgiler veritabanına kaydedildi";
                    }
                    else
                    {
                        ViewBag.durum = "Resim dosyası seçmeniz gerekiyor. Yanlış tip";
                        return View(slider);
                    }
                }

                var sliderToUpdate = db.Sliders.Find(id);
                if (sliderToUpdate == null)
                {
                    return HttpNotFound();
                }

                sliderToUpdate.slider_title = slider.slider_title;
                sliderToUpdate.slider_description = slider.slider_description;

                if (file != null && file.ContentLength > 0)
                {
                    sliderToUpdate.slider_img = dosya_adi;
                }

                TryUpdateModel(sliderToUpdate);
                db.SaveChanges();

                return RedirectToAction("slider_listesi");
            }
            return View(slider);
        }
        /*******************************************************************************/

        /*Sosyal Medya*/
        public ActionResult sosyalmedya_listesi()
        {
            var socialmedia = db.SocialMedia.ToList();
            return View(socialmedia);
        }
        public async Task<ActionResult> sosyalmedya_ekle()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> sosyalmedya_ekle(SocialMedia socialMedia)
        {
            if (ModelState.IsValid)
            {

                var socialmedia = db.SocialMedia.Add(socialMedia);
                db.SaveChanges();
                return View(socialmedia);
            }
            else
            { return View("sosyalmedya_listesi"); }
        }

        public async Task<ActionResult> sosyalmedya_sil(int? id)
        {

            SocialMedia socialMedia = await db.SocialMedia.FindAsync(id);
            db.SocialMedia.Remove(socialMedia);
            await db.SaveChangesAsync();
            return RedirectToAction("sosyalmedya_listesi", "Admin");

        }
        public async Task<ActionResult> sosyalmedya_guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialMedia socialMedia = await db.SocialMedia.FindAsync(id);
            if (socialMedia == null)
            {
                return HttpNotFound();
            }
            return View(socialMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> sosyalmedya_guncelle([Bind(Include = "id,instagram_url,twitter_url,facebook_url,discord_url,linkedin_url")] SocialMedia socialMedia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(socialMedia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("sosyalmedya_listesi", "Admin");
            }
            return View(socialMedia);
        }

        /********************************************************/
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}