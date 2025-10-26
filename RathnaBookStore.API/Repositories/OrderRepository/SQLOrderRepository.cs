using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Repositories.OrderRepository
{
    public class SQLOrderRepository : IOrderRepository
    {
        private readonly BookStoreDbContext dbContext;

        public SQLOrderRepository(BookStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Create Order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                //Reduce Quantity of book for each ordered book
                foreach(var orderItem in order.OrderItems)
                {
                    var book = await dbContext.Books.FindAsync(orderItem.BookId);

                    if(book == null)
                    {
                        throw new Exception($"Not Found Book With Id {orderItem.BookId}");
                    }

                    if(book.Quantity < orderItem.Quantity)
                    {
                        throw new Exception($"Insuffient stock for book '{book.Name}'");
                    }

                    //Reduce book quantity
                    book.Quantity -= orderItem.Quantity;
                }

                await dbContext.Orders.AddAsync(order);
                await dbContext.SaveChangesAsync();

                //Commit transcation
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //Get All Orders
        public async Task<List<Order>> GetOrdersAsync(DateTime? filterStartDate = null, DateTime? filterEndDate = null,
            string? sortBy = null, bool isAcsending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //Filtering
            var orders = dbContext.Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .AsQueryable();

            if(filterStartDate.HasValue)
            {
                orders = orders.Where(o => o.OrderTime >= filterStartDate.Value);
            }

            if(filterEndDate.HasValue)
            {
                var endDateInclusive = filterEndDate.Value.Date.AddDays(1);
                orders = orders.Where(o => o.OrderTime < endDateInclusive);
            }

            //sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("OrderTime", StringComparison.OrdinalIgnoreCase))
                {
                    orders = isAcsending ? orders.OrderBy(o => o.OrderTime) : orders.OrderByDescending(o => o.OrderTime);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await orders.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        //Get Orders By id
        public async Task<Order?> GetOrdersByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await dbContext.Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book).FirstOrDefaultAsync(o => o.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        //Update Order
        public async Task<Order?> UpdateOrderAsync(Guid id, Order Updatedorder)
        {
            try
            {
                //check order is existing
                var existingOrder = await dbContext.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    return null;
                }

                foreach (var updatedOrderItem in Updatedorder.OrderItems)
                {
                    var existingItem = existingOrder.OrderItems.FirstOrDefault(oi => oi.BookId == updatedOrderItem.BookId);

                    if (existingItem == null)
                    {
                        throw new Exception("Order item not found.");
                    }

                    var book = await dbContext.Books.FindAsync(existingItem.BookId);

                    if (book == null)
                    {
                        throw new Exception($"'{book?.Name}' Book not Found");
                    }

                    int quantityDifferent = updatedOrderItem.Quantity - existingItem.Quantity;

                    if (book.Quantity < quantityDifferent)
                    {
                        throw new Exception("Insuffient stock!");
                    }

                    book.Quantity -= quantityDifferent;

                    //update order
                    existingItem.Quantity = updatedOrderItem.Quantity;
                    existingItem.UnitPrice = (decimal)book.Price;

                }

                existingOrder.OrderTime = DateTime.UtcNow;
                existingOrder.Discount = Updatedorder.Discount;
                existingOrder.SubTotal = existingOrder.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
                existingOrder.TotalAmount = existingOrder.SubTotal - existingOrder.Discount;

                await dbContext.SaveChangesAsync();
                return existingOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
