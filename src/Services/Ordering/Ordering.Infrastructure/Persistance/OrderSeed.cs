using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistance
{
    public class OrderSeed
    {
        

        public async static Task SeedOrderAsync(OrderContext _context, ILogger<OrderSeed> _logger)
        {
            if (!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(GetPreconfiguredOrders());
                await _context.SaveChangesAsync();
                _logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }
        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "Mohamed", FirstName = "Mohamed", LastName = "Sameh", EmailAddress = "mohamed@gmail.com", AddressLine = "Cairo", Country = "Egypt", TotalPrice = 350 }
            };
        }


    }
}
