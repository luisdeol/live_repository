using LiveRepository.App.DomainInterfaces;
using LiveRepository.App.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LiveRepository.App.Controllers
{
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryRepository _deliveryRepository;
        public DeliveriesController(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();

            return Ok(deliveries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);

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

            delivery.Id = await _deliveryRepository.AddAsync(delivery);

            return CreatedAtAction(nameof(GetById), new { id = delivery.Id }, delivery);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> CompleteDelivery(int id)
        {
            await _deliveryRepository.Complete(id);

            return NoContent();
        }
    }
}
