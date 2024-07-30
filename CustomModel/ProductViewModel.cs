using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ValpeVerkkokauppa.Models;

namespace ValpeVerkkokauppa.CustomModel
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}