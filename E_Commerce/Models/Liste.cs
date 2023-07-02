using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AhmetProj.Models
{
    public class Liste
    {
        //Bu liste class ım projemde kullanmıs oldugum veri koleksiyonlarımı barındırıyor. bu sınıfın içindeki veri koleksiyonlarının dolduruluyor ve ilgili özelliklerin kullanılarak verilere erişildiği bir classtır
        ///veya() yapıdır.
        public List<Sliders> sliders { get; set; }
        
        public List<Orders> orders { get; set; }
       
        public List<Categorys> categorys { get; set; }
        public List<Products> products { get; set; }
       public Cart Cart { get; set; }
        public List<Carts> carts { get; set; }
        public List<Brands> brands { get; set; }

        

      
    }
  
    
}