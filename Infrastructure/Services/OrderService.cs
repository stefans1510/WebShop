using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICartRepository cartRepository;

        public OrderService(IUnitOfWork UnitOfWork, ICartRepository cartRepository)
        {
            unitOfWork = UnitOfWork;
            this.cartRepository = cartRepository;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string cartId, Address shippingAddress)
        {
            var cart = await cartRepository.GetCartAsync(cartId);
            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(
                    productItem.Id, 
                    productItem.Name, 
                    productItem.PictureUrl
                );
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var specification = new OrderByPaymentIntentIdSpecification(cart.PaymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);
            
            if (order != null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subtotal;
                unitOfWork.Repository<Order>().Update(order);
            }
            else
            {
                order = new Order(
                    items, 
                    buyerEmail, 
                    shippingAddress, 
                    deliveryMethod,
                    subtotal, 
                    cart.PaymentIntentId
                );
                unitOfWork.Repository<Order>().Add(order);
            }

            var result = await unitOfWork.Complete();  //save to db

            if (result <= 0) return null;

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specification = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var specification = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await unitOfWork.Repository<Order>().ListAsync(specification);
        }
    }
}