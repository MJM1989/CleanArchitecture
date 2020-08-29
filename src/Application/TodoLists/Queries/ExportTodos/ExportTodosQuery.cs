using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Queries.ExportTodos
{
    public class ExportTodosQuery : IRequest<ExportTodosVm>
    {
        public int ListId { get; set; }
    }

    public class ExportTodosQueryHandler : IRequestHandler<ExportTodosQuery, ExportTodosVm>
    {
        private readonly ITodoItemStore todoItemStore;
        private readonly IMapper mapper;
        private readonly ICsvFileBuilder fileBuilder;

        public ExportTodosQueryHandler(ITodoItemStore todoItemStore, IMapper mapper, ICsvFileBuilder fileBuilder)
        {
            this.todoItemStore = todoItemStore;
            this.mapper = mapper;
            this.fileBuilder = fileBuilder;
        }

        public async Task<ExportTodosVm> Handle(ExportTodosQuery request, CancellationToken cancellationToken)
        {
            var vm = new ExportTodosVm();

            IEnumerable<TodoItem> items = await todoItemStore.GetByListId(request.ListId);
            
            List<TodoItemRecord> records = mapper.Map<IEnumerable<TodoItemRecord>>(items).ToList();

            vm.Content = fileBuilder.BuildTodoItemsFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = "TodoItems.csv";

            return await Task.FromResult(vm);
        }
    }
}
