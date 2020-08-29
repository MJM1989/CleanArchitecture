using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommand : IRequest<int>
    {
        public string Title { get; set; }
    }

    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
    {
        private readonly ITodoListStore todoListStore;

        public CreateTodoListCommandHandler(ITodoListStore todoListStore)
        {
            this.todoListStore = todoListStore;
        }

        public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoList { Title = request.Title };


            return await todoListStore.InsertAsync(entity);
        }
    }
}
