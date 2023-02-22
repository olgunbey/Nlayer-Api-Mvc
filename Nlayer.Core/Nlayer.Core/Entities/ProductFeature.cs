using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Core.Entities
{
    public class ProductFeature
    {
        public int ID { get; set; }
        public string? Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ProductID { get; set; }
        public Product? Product { get; set; }

    }
}
