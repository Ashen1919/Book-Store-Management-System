using AutoMapper;
using RathnaBookStore.API.Models.Domains;
using RathnaBookStore.API.Models.DTO.BookDto;
using RathnaBookStore.API.Models.DTO.CategoryDto;
using RathnaBookStore.API.Models.DTO.Order;
using RathnaBookStore.API.Models.DTO.OrderDto;
using RathnaBookStore.API.Models.DTO.OrderItemDto;

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

            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<AddBookRequestDto, Book>().ReverseMap();
            CreateMap<UpdateBookRequestDto, Book>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>().ReverseMap();

            CreateMap<OrderItem, OrderItemDtos>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Name));
            CreateMap<CreateOrderItemDto, OrderItem>();

        }
    }
}
