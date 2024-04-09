using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(
            ICartRepository cartRepository, 
            IUnitOfWork unitOfWork, 
            IConfiguration configuration
        )
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        public async Task<CustomerCart> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var cart = await cartRepository.GetCartAsync(cartId);

            if (cart == null) return null;
            
            var shippingPrice = 0m;

            if  (cart.DeliveryMethodId.HasValue) 
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)cart.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.Items)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if(item.Price != productItem.Price) {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long) cart.Items.Sum(i => i.Quantity * (i.Price * 100))
                        + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) cart.Items.Sum(i => i.Quantity * (i.Price * 100))
                        + (long) shippingPrice * 100,
                };
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            await cartRepository.UpdateCartAsync(cart);

            return cart;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specification = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentFailed;
            await unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var specification = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentRecieved;
            await unitOfWork.Complete();

            return order;
        }
    }
}