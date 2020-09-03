using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class AttributeBLL
    {
        private static IAttributeDAL AttributeDB;

        public static void  Initialize(string connectionString)
        {
            AttributeDB = new AttributeDAL(connectionString);
        }
        public static List<DomainModels.Attribute> getAll(int CategoryID)
        {
            return AttributeDB.getAll(CategoryID);
        }
    }
}
