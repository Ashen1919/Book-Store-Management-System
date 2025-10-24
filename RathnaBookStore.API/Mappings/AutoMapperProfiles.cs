using AutoMapper;
using RathnaBookStore.API.Models.Domains;
using RathnaBookStore.API.Models.DTO.CategoryDto;

namespace RathnaBookStore.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        // Ensure this class is not abstract and has a public parameterless constructor
        public AutoMapperProfiles()
        {
            // Add your mapping configurations here
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<AddCategoryRequestDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryRequestDto, Category>().ReverseMap();
        }
    }
}
