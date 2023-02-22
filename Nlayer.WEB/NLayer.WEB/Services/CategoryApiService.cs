using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using System.Linq.Expressions;

namespace NLayer.WEB.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient httpClient;
        public CategoryApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<CategoryDTO> Save(CategoryDTO categoryDto)
        {
        HttpResponseMessage response=  await httpClient.PostAsJsonAsync("Categories",categoryDto);
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                CustomResponseDTO<CategoryDTO> responsebody = await response.Content.ReadFromJsonAsync<CustomResponseDTO<CategoryDTO>>();
                return responsebody.Data;
            }
        }
        public async Task<CategoryDTO> GetByID(int categoryid)
        {
           return await httpClient.GetFromJsonAsync<CategoryDTO>($"categories/{categoryid}");
        }
        //public async Task<CategoryDTO> Where(Expression<Func<CategoryDTO,bool>> expression)
        //{
        //   return await httpClient.GetFromJsonAsync<CategoryDTO>($"categories/CategoriesWhere/{}");
        //}
    }
}
