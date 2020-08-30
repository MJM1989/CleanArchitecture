using AutoMapper;
using CleanArchitecture.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos
{
    public class GetTodosQuery : IRequest<TodosVm>
    {
    }

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, TodosVm>
    {
        private readonly ITodoListStore todoListStore;
        private readonly IMapper mapper;

        public GetTodosQueryHandler(ITodoListStore todoListStore, IMapper mapper)
        {
            this.todoListStore = todoListStore;
            this.mapper = mapper;
        }

        public async Task<TodosVm> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            return new TodosVm
            {
                PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                    .Cast<PriorityLevel>()
                    .Select(p => new PriorityLevelDto { Value = (int)p, Name = p.ToString() })
                    .ToList(),

                Lists = mapper.Map<IEnumerable<TodoListDto>>(await todoListStore.GetAllWithItemsAsync())
                    .OrderBy(t => t.Title)
                    .ToList()
            };
        }
    }
}
