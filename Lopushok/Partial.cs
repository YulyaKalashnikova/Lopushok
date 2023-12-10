using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lopushok
{
    public partial class Product
    {
        public decimal Cost
        {
            get
            {
                return ProductMaterial.Where(x=>x.ProductID==ID).Sum(x=>x.Material.Cost);
            }
        }
        public string ImagePath
        {
            get
            {
                string imagePath = string.Empty;
                if (Image==string.Empty)
                {
                    imagePath ="picture.png";
                }
                else if (Image!=string.Empty)
                {
                    imagePath = Environment.CurrentDirectory + Image;
                }
                return imagePath;
            }
        }
    }
}
