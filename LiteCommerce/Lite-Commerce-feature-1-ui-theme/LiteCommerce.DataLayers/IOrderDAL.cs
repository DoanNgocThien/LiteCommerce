using System.Collections.Generic;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers
{
    public interface IOrderDAL
    {
        List<Order> List(int page, int pageSize, string country, string category, string employee, string shipper);

        int Count(string country, string category, string employee, string shipper);

        Order Get(int orderID);

        int Add(Order order);

        //bool Update(Order order);

        int Delete(int[] orderIDs);
    }
}