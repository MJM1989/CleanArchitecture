using AutoMapper;
using CleanArchitecture.Domain.Entities;
using DbTodoList = CleanArchitecture.Infrastructure.Persistence.Entities.TodoList;
using DbTodoItem = CleanArchitecture.Infrastructure.Persistence.Entities.TodoItem;

namespace CleanArchitecture.Infrastructure.Persistence.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DbTodoList, TodoList>()
                .ForMember(dest => dest.Items, opt =>
                    opt.MapFrom(src => src.Items));
            CreateMap<TodoList, DbTodoList>();

            CreateMap<DbTodoItem, TodoItem>();
            CreateMap<TodoItem, DbTodoItem>();
        }
    }
}