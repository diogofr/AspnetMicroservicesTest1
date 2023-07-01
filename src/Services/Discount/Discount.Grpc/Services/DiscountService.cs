using AutoMapper;
using Discount;
using Discount.Common.Entities;
using Discount.Common.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;

        public DiscountService(ILogger<DiscountService> logger, IDiscountRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName}"));
            }
            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            bool success = await _repository.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. ProductName: {productName}", coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            bool success = await _repository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. ProductName: {productName}", coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            bool success = await _repository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse { Sucess = success };  
        }

        

        /*
        public override Task<HelloReply> GetDiscount(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }*/
    }
}