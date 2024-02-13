using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ProductInventoryManagement.Models
{
    public class ProductBusinessLogic
    {
        public static List<Product> Products = new List<Product>()
        {
            new Product() { ProductID = 1, Description = "20x10 box of Chocolates", Name = "Swiss Chocolate", Price = 250 },
            new Product() { ProductID = 2, Description = "Iodized salt", Name = "Salt", Price = 300 },
            new Product() { ProductID = 3, Description = "Granulated sugar", Name = "Sugar", Price = 350 }
        };

        public static List<Inventory> Inventory = new List<Inventory>()
        {
            new Inventory() { ProductID = 1, InventoryID = 11, StockQuantity = 350 },
            new Inventory() { ProductID = 1, InventoryID = 12, StockQuantity = 350 },
            new Inventory() { ProductID = 2, InventoryID = 13, StockQuantity = 350 },
            new Inventory() { ProductID = 2, InventoryID = 14, StockQuantity = 500 },
            new Inventory() { ProductID = 3, InventoryID = 15, StockQuantity = 650 }
        };

        
        // Join Products and Inventory lists to return full product information
        public List<ProductInventory> GetProductsInfo()
        {
            List<ProductInventory> productInventory = Products.GroupJoin(Inventory,
                                                Products => Products.ProductID,
                                                Inventory => Inventory.ProductID,
                                                (Products, Inventory) => new ProductInventory
                                                {
                                                    Name = Products.Name,
                                                    Description = Products.Description,
                                                    Price = Products.Price,
                                                    StockQuantity = Inventory.Select(inv => inv.StockQuantity).Sum()
                                                }).ToList();

            return productInventory;
        }

        // Add Inventory and corresponding Product data to database collections
        public bool AddProductInventory(ProductInventory productInventory)
        {
            if (productInventory.Name != null && productInventory.StockQuantity != 0
                && productInventory.Name.Trim() != "")
            {
                // Checks if the product exists in database
                if (Products.Any(product => product.Name.ToLower() == productInventory.Name.ToLower().Trim()))
                {
                    // Updates the inventory collection with new inventory transaction
                    Inventory.Add(new Inventory()
                    {
                        ProductID = Products.Find(product => 
                                                        product.Name.ToLower() == productInventory.Name.ToLower().Trim()).ProductID,
                        InventoryID = Inventory.Last().InventoryID + 1,
                        StockQuantity = productInventory.StockQuantity
                    });
                    return true;
                }
                else
                {
                    // Add new product to Products collection
                    int nextProductId = Products.Any() ? Products.Last().ProductID + 1 : 0;

                    Products.Add(new Product()
                    {
                        ProductID = nextProductId,
                        Name = productInventory.Name,
                        Description = productInventory.Description,
                        Price = productInventory.Price,
                    });

                    // Update Inventory collection
                    int nextInventoryId = Inventory.Any() ? Inventory.Last().InventoryID + 1 : 0;

                    Inventory.Add(new Inventory()
                    {
                        ProductID = Products.Find(product => product.Name.ToLower() 
                                                                            == productInventory.Name.ToLower().Trim()).ProductID,
                        InventoryID = nextInventoryId,
                        StockQuantity = productInventory.StockQuantity
                    });

                    return true;
                }
            } 
            else
            {
                return false;
            }          
        }

        // Delete a product from Product and Inventory lists
        public void DeleteProduct(string productName)
        {
            if (string.IsNullOrEmpty(productName) || 
                !Products.Any(product => product.Name.ToLower() == productName.ToLower().Trim())) 
            { return; }

            int productIdToRemove = Products.Find(product => product.Name.ToLower() == productName.ToLower().Trim()).ProductID;
            Products.Remove(Products.Find(product => product.ProductID == productIdToRemove));
            Inventory.RemoveAll(inventory => inventory.ProductID == productIdToRemove);
        }
    }
}