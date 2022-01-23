using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        public const string BasePath = "api/v1/product";
        private readonly HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client;
        }


        public async Task<ProductViewModel> CreateProduct(ProductViewModel product, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(BasePath, product);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling API");

            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<bool> DeleteProduct(long id, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling API");

            return await response.ReadContentAs<bool>();
        }

        public async Task<IEnumerable<ProductViewModel>> FindAllProducts(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync(BasePath);
            return await response.ReadContentAs<List<ProductViewModel>>();
        }

        public async Task<ProductViewModel> FindProductById(long id, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel product, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PutAsJsonAsync(BasePath, product);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Something went wrong when calling API");

            return await response.ReadContentAs<ProductViewModel>();
        }
    }
}
