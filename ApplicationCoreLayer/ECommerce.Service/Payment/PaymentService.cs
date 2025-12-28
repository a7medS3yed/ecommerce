using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Service.Abstraction.Payment;
using ECommerce.Shared.CommenResponse;
using ECommerce.Shared.Dtos.Baskets;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = ECommerce.Domain.Entities.ProductModule.Product;

namespace ECommerce.Service.Payment
{
    internal class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IBasketRepository basketRepository, IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var sKey = _configuration["Stripe:SKey"];
            if (sKey is null)
                return Error.Faliure("Faild to obtain secret key value");
            StripeConfiguration.ApiKey = sKey;

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                return Error.NotFound("Basket.NotFound", $"Basket with this id {basketId} is not found");

            if (basket.DeliveryMethodId is null)
                return Error.Validation("Delivery Method is not selected to basket");

            var method = await _unitOfWork.GenaricRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value);

            if (method is null)
                return Error.NotFound("Delivery Method is Not Found");

            basket.ShippingPrice = method.Price;

            foreach(var item in basket.BasketItems)
            {
                var product = await _unitOfWork.GenaricRepository<Product, int>()
                                               .GetByIdAsync(item.Id);

                if (product is null)
                    return Error.NotFound("Product is not Found");

                item.Price = product.Price;
                item.ProductName = product.Name;
                item.PictureUrl = product.PictureUrl;
            }

            long amount =(long) (basket.BasketItems.Sum(i => i.Quentity * i.Price)*100);

            var service = new PaymentIntentService();

            if(basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };

                var intent = await service.CreateAsync(options);

                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;

            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };

                await service.UpdateAsync(
                    basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
