using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductInventoryManagement.Models
{
    public class Inventory
    {
        // Inventory properties
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int StockQuantity { get; set; }
    }
}