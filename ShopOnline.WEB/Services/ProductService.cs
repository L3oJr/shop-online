using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product");

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>();
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await httpClient.GetAsync("api/Product/GetProductCategories");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HttpStatusCode = {response.StatusCode} Message - {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
