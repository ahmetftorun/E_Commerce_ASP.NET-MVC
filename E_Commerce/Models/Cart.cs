using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AhmetProj.Models
{
    public class Cart
    {
        private List<Carts> _sepetim = new List<Carts>();

        public List<Carts> Sepetim { get => _sepetim; }

        public void Sepete_ekle(Products gelen_urun, byte adet)
        {
            var varolan_urun = _sepetim.FirstOrDefault(x => x.urun.product_id == gelen_urun.product_id);
            if (varolan_urun == null)
            {
                _sepetim.Add(new Carts { urun = gelen_urun, adet = 1 });
            }
            else if (adet == 0)
            {
                varolan_urun.adet += 1;
            }
            else
            {
                varolan_urun.adet = adet;
            }
        }

        public void Sepetten_sil(Products silinecek_urun)
        {
            _sepetim.RemoveAll(x => x.urun.product_id == silinecek_urun.product_id);
        }

        public double Sepet_toplami()
        {
            return _sepetim.Sum(x => Convert.ToDouble(x.urun.product_price * x.adet));
        }

        public void Sepeti_temizle()
        {
            _sepetim.Clear();
        }
    }
}