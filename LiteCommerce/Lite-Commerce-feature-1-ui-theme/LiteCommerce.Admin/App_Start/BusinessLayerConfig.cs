using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
    public class BusinessLayerConfig
    {
        public static void Init()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerce"].ConnectionString;
            //TODO : Khởi tạo các BLL khi cần sử dụng đến
            CatalogBLL.Initialize(connectionString);
            EmloyeeBLL.Initialize(connectionString);
            OrderBLL.Initialize(connectionString);
            CountryBLL.Initialize(connectionString);
            AttributeBLL.Initialize(connectionString);
            ProductAttributeBLL.Initialize(connectionString);
            UserAccountBLL.Initialize(connectionString);
        }
    }
}