using Microsoft.EntityFrameworkCore;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.Domains;

namespace RathnaBookStore.API.Repositories.CategoryRepository
{
    public class SQLCategoryRepository : ICategoryRepository
    {
        private readonly BookStoreDbContext dbContext;

        public SQLCategoryRepository(BookStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Create a category
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await dbContext.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        //Delete Category
        public async Task<Category?> DeleteCategoryAsync(Guid id)
        {
            //check category is available
            var existCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(existCategory == null)
            {
                return null;
            }

            dbContext.Categories.Remove(existCategory);
            await dbContext.SaveChangesAsync();

            return existCategory;
        }

        //Get All categories
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        //Update Category
        public async Task<Category?> UpdateCategoryAsync(Guid id, Category category)
        {
            var existCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existCategory == null)
            {
                return null;
            }

            existCategory.Name = category.Name;
            existCategory.ImageUrl = category.ImageUrl;

            await dbContext.SaveChangesAsync();
            return existCategory;
        }

    }
}
