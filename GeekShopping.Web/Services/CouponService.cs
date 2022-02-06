using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class CouponService : ICouponService
    {
        public const string BasePath = "api/v1/coupon";
        private readonly HttpClient _client;

        public CouponService(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponViewModel> GetCoupon(string couponCode, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{BasePath}/{couponCode}");
            if (!response.IsSuccessStatusCode)
                return new CouponViewModel();

            return await response.ReadContentAs<CouponViewModel>();
        }
    }
}
