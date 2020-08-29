using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList
{
    public class DeleteTodoListCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
    {
        private readonly ITodoListStore todoListStore;

        public DeleteTodoListCommandHandler(ITodoListStore todoListStore)
        {
            this.todoListStore = todoListStore;
        }

        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            TodoList entity = await todoListStore.GetAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            await todoListStore.Delete(entity);

            return Unit.Value;
        }
    }
}
