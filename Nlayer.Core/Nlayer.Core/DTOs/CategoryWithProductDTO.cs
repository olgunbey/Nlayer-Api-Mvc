using Nlayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Core.DTOs
{
    public class CategoryWithProductDTO:CategoryDTO
    {
        public ICollection<ProductDTO>? Products { get; set; }
    }
}
