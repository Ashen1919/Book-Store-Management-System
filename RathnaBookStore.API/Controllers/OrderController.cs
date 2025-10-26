using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.Domains;
using RathnaBookStore.API.Models.DTO.Order;
using RathnaBookStore.API.Models.DTO.OrderDto;
using RathnaBookStore.API.Repositories.OrderRepository;

namespace RathnaBookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BookStoreDbContext dbContext;
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderController(BookStoreDbContext dbContext, IOrderRepository orderRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        //Get All Orders
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] DateTime? filterStartDate = null,
            [FromQuery] DateTime? filterEndDate = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageSize = 1000)
        {
            var orderDomainModel = await orderRepository.GetOrdersAsync(
                filterStartDate, filterEndDate, sortBy, isAscending, pageNumber, pageSize);

            var orderDto = mapper.Map<List<OrderDto>>(orderDomainModel);

            return Ok(orderDto);
        }

        //Get Orders By Id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrdersById([FromRoute] Guid id)
        {
            var orderDomainModel = await orderRepository.GetOrdersByIdAsync(id);

            if(orderDomainModel == null)
            {
                return NotFound();
            }

            var orderDto = mapper.Map<OrderDto>(orderDomainModel);

            return Ok(orderDto);
        }

        //Create Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            //Validate books are exist and get their prices
            var bookIds = createOrderDto.OrderItems.Select(oi => oi.BookId).ToList();
            var books = await dbContext.Books.Where(b => bookIds.Contains(b.Id)).ToListAsync();

            //check all books exist
            if(books.Count != bookIds.Count)
            {
                return BadRequest("One or More Books not found");
            }

            //Map Dto to domain model
            var orderDomainModel = mapper.Map<Order>(createOrderDto);

            //set unique IDs
            orderDomainModel.Id = Guid.NewGuid();
            orderDomainModel.OrderTime = DateTime.Now;

            //Process order Items
            foreach(var orderItem in orderDomainModel.OrderItems)
            {
                orderItem.Id = Guid.NewGuid();

                var book = books.First(b => b.Id == orderItem.BookId);
                orderItem.UnitPrice = (decimal)book.Price;
            }

            //Calculate Totals
            orderDomainModel.SubTotal = orderDomainModel.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
            orderDomainModel.TotalAmount = orderDomainModel.SubTotal - orderDomainModel.Discount;

            //Use domain model to create order
            orderDomainModel = await orderRepository.CreateOrderAsync(orderDomainModel);

            //Reload with navigation properties for mapping
            orderDomainModel = await orderRepository.GetOrdersByIdAsync(orderDomainModel.Id);

            //Map domain model to Dto
            var orderDto = mapper.Map<OrderDto>(orderDomainModel);

            return CreatedAtAction(nameof(GetOrdersById), new { id = orderDto.Id }, orderDto);
        }

        //Update Order
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderDto updateOrderDto)
        {
           
            if(updateOrderDto == null || updateOrderDto.OrderItems == null || !updateOrderDto.OrderItems.Any())
            {
                return BadRequest("Inavlid Order Data");
            }

            try
            {
                var updateOrderDomainModel = mapper.Map<Order>(updateOrderDto);

                var result = await orderRepository.UpdateOrderAsync(id, updateOrderDomainModel);

                if(result == null)
                {
                    return NotFound($"Order with ID '{id}' not found");
                }

                return Ok(result);

            }catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message });
            }
        }

    }
}
