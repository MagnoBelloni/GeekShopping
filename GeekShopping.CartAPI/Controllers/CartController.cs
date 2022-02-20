using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _coupounRepository;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository cartRepository,
            ICouponRepository coupounRepository,
            IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _cartRepository = cartRepository;
            _coupounRepository = coupounRepository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> FindById(string id)
        {
            var cart = await _cartRepository.FindCartByUserId(id);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        
        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart([FromBody]CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        
        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        
        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);
            if (!status)
                return BadRequest();

            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon([FromBody] ApplyCouponVO vo)
        {
           
            var status = await _cartRepository.ApplyCoupon(vo.UserId, vo.CouponCode);
            if (!status)
                return NotFound();

            return Ok(status);
        }
        
        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status)
                return NotFound();

            return Ok(status);
        }
        
        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"];

            if (vo?.UserId == null)
                return BadRequest();

            var cart = await _cartRepository.FindCartByUserId(vo.UserId);
            if (cart == null)
                return NotFound();

            if (!string.IsNullOrEmpty(vo.CouponCode))
            {
                CouponVO coupon = await _coupounRepository.GetCoupon(vo.CouponCode, token);
                if(vo.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(412);
                }
            }

            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Now;

            _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

            await _cartRepository.ClearCart(vo.UserId);

            return Ok(vo);
        }

        [HttpDelete("clear-cart/{userId}")]
        public async Task<ActionResult> Checkout(string userId)
        {
            await _cartRepository.ClearCart(userId);

            return Ok();
        }
    }
}