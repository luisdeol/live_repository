using Dapper;
using LiveRepository.App.Entities;
using LiveRepository.App.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LiveRepository.App.Controllers
{
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly DeliveryAppContext _deliveryAppContext;
        public DeliveriesController(DeliveryAppContext deliveryAppContext)
        {
            _deliveryAppContext = deliveryAppContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryAppContext.Deliveries.ToListAsync();

            return Ok(deliveries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var delivery = await _deliveryAppContext.Deliveries.SingleOrDefaultAsync(d => d.Id == id);

            if (delivery == null)
            {
                return NotFound();
            }

            return Ok(delivery);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Delivery delivery)
        {
            delivery.TotalPrice = delivery.Items.Sum(i => i.Quantity * i.Price);

            var script = @"INSERT INTO Deliveries (MotorcycleCourier, TotalPrice) 
                OUTPUT INSERTED.Id
                VALUES (@MotorcycleCourier, @TotalPrice);";

            using (var sqlConnection = new SqlConnection(_deliveryAppContext.Database.GetConnectionString()))
            {
                delivery.Id = await sqlConnection.QuerySingleAsync<int>(script, new { delivery.MotorcycleCourier, delivery.TotalPrice });
            }

            return CreatedAtAction(nameof(GetById), new { id = delivery.Id }, delivery);
        }
    }
}
