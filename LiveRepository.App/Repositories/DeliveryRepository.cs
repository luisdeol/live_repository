using Dapper;
using LiveRepository.App.DomainInterfaces;
using LiveRepository.App.Entities;
using LiveRepository.App.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveRepository.App.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryAppContext _context;
        public DeliveryRepository(DeliveryAppContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Delivery delivery)
        {
            var script = @"INSERT INTO Deliveries (MotorcycleCourier, TotalPrice) 
                OUTPUT INSERTED.Id
                VALUES (@MotorcycleCourier, @TotalPrice);";

            var scriptItems = @"INSERT INTO DeliveryItems (ProductName, Price, Quantity, DeliveryId) 
                VALUES (@ProductName, @Price, @Quantity, @DeliveryId);";

            using (var sqlConnection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    delivery.Id = await sqlConnection.QuerySingleAsync<int>(script, new { delivery.MotorcycleCourier, delivery.TotalPrice }, transaction: transaction);

                    foreach (var deliveryItem in delivery.Items)
                    {
                        await sqlConnection.ExecuteAsync(scriptItems, new { deliveryItem.ProductName, deliveryItem.Price, deliveryItem.Quantity, DeliveryId = delivery.Id }, transaction: transaction);
                    }

                    transaction.Commit();
                }
            }

            return delivery.Id;
        }

        public async Task Complete(int id)
        {
            var script = "UPDATE Deliveries SET Completed = 1 WHERE Id = @id";

            using (var sqlConnection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                sqlConnection.Open();

               await sqlConnection.ExecuteAsync(script, new { id });
            }
        }

        public async Task<List<Delivery>> GetAllAsync()
        {
            return await _context.Deliveries.ToListAsync();
        }

        public async Task<Delivery> GetByIdAsync(int id)
        {
            return await _context.Deliveries.SingleOrDefaultAsync(d => d.Id == id);
        }
    }
}
