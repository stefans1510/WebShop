using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCartById(string id)
        {
            var cart = await cartRepository.GetCartAsync(id);

            return Ok(cart ?? new CustomerCart(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerCart>> UpdateCart(CustomerCart cart)
        {
            var updatedCart = await cartRepository.UpdateCartAsync(cart);

            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task DeleteCart(string id)
        {
            await cartRepository.DeleteCartAsync(id);
        }
    }
}