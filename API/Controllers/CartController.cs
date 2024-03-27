using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;

        public CartController(ICartRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCartById(string id)
        {
            var cart = await cartRepository.GetCartAsync(id);

            return Ok(cart ?? new CustomerCart(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerCart>> UpdateCart(CustomerCartDto cart)
        {
            var customerCart = mapper.Map<CustomerCartDto, CustomerCart>(cart);
            var updatedCart = await cartRepository.UpdateCartAsync(customerCart);

            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task DeleteCart(string id)
        {
            await cartRepository.DeleteCartAsync(id);
        }
    }
}