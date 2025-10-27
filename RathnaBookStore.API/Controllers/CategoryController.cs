using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RathnaBookStore.API.Data;
using RathnaBookStore.API.Models.Domains;
using RathnaBookStore.API.Models.DTO.CategoryDto;
using RathnaBookStore.API.Repositories.CategoryRepository;

namespace RathnaBookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BookStoreDbContext dbContext;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(BookStoreDbContext dbContext, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        //Create Book Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryRequestDto addCategoryRequestDto)
        {
            //Map DTO to domain model
            var categoryDomainModel = mapper.Map<Category>(addCategoryRequestDto);

            //Use domain model to create Category
            categoryDomainModel = await categoryRepository.CreateCategoryAsync(categoryDomainModel);

            //Map domain model back to Dto
            var categoryDto = mapper.Map<CategoryDto>(categoryDomainModel);

            return Ok(categoryDto);
        }

        //Get All categories
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategories()
        {
            //Get categories from repository
            var categoryDomainModel = await categoryRepository.GetCategoriesAsync();

            //Map domain model to Dto
            var categoryDto = mapper.Map<List<CategoryDto>>(categoryDomainModel);

            return Ok(categoryDto);
        }

        //Update category
        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            //Map Dto to domain model
            var categoryDomainModel = mapper.Map<Category>(updateCategoryRequestDto);

            categoryDomainModel = await categoryRepository.UpdateCategoryAsync(id, categoryDomainModel);

            //check if category is exist
            if (categoryDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to Dto
            var categoryDto = mapper.Map<CategoryDto>(categoryDomainModel);

            return Ok(categoryDto);
        }

        //Delete Category
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var categoryDomainModel = await categoryRepository.DeleteCategoryAsync(id);

            if(categoryDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to Dto
            var categoryDto = mapper.Map<CategoryDto>(categoryDomainModel);

            return Ok(categoryDto);
        }
    }
}
