using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;

namespace NLayer.WEB.Services
{
    public class ProductApiService
    {
        private readonly HttpClient httpClient;
        public ProductApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<ProductCategoryDTO>> GetProductsWithCategory()
        {
            CustomResponseDTO<List<ProductCategoryDTO>> response = await httpClient.GetFromJsonAsync<CustomResponseDTO<List<ProductCategoryDTO>>>("products/GetProductsCategory");
            return response.Data;

        }
        public async Task<ProductDTO> Save(ProductDTO productDTO)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("products",productDTO);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDTO<ProductDTO>>();
                return responseBody.Data;
            }
        }
        public async Task<ProductDTO> GetByID(int id)
        {
            var response = await httpClient.GetFromJsonAsync<CustomResponseDTO<ProductDTO>>($"products/{id}");
            return response.Data;
        }
        public async Task<bool> UpdateAsync(ProductDTO productDTO)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync("products",productDTO);
            return response.IsSuccessStatusCode;

        }
        public async Task<bool> RemoveAsync(int id)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"products/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
