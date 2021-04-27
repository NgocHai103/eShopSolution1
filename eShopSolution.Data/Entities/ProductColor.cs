using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductColor
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Color color { get; set; }
        public Product Product { get; set; }

    }
}
