using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync (Category category);
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> UpdateCategoryAsync(Guid id,  Category category);
        Task<Category?> DeleteCategoryAsync(Guid id); 
    }
}
