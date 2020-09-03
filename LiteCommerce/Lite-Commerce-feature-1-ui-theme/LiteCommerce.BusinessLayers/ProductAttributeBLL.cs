using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class ProductAttributeBLL
    {
        private static IProductAttributesDAL ProductAttributeDB;
        public static void Initialize(string connectionString)
        {
            ProductAttributeDB = new ProductAttributeDAL(connectionString);
        }
        public static List<ProductAttributes> getAll(int productID)
        {
            return ProductAttributeDB.getAll(productID);
        }
        public static int Add(List<ProductAttributes> ProductAttributes)
        {
            return ProductAttributeDB.Add(ProductAttributes);
        }
        public static int Update(List<ProductAttributes> ProductAttributes)
        {
            return ProductAttributeDB.Update(ProductAttributes);
        }
        public static int Delete(int[] ProductIDs)
        {
            return ProductAttributeDB.Delete(ProductIDs);
        }
        public static bool CheckProductInAttibute(int ProductID)
        {
            return ProductAttributeDB.CheckProductInAttibute(ProductID);
        }
    }
}
