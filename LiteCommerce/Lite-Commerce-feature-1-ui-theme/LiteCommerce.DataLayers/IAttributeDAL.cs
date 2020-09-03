using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IAttributeDAL
    {
        List<DomainModels.Attribute> getAll(int CategoryID);
    }
}
