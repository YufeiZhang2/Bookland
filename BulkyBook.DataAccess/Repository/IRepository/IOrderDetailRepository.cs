using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}