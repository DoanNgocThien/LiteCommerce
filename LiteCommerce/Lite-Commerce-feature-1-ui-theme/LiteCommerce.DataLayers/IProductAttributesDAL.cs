using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IProductAttributesDAL
    {
        List<ProductAttributes> getAll(int productID);
        int Delete(int[] ProductIDs);
        int Update(List<ProductAttributes> data);
        int Add(List<ProductAttributes> data);
        bool CheckProductInAttibute(int ProductID);
    }
}
