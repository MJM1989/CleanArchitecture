using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem
{
    public class DeleteTodoItemCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
    {
        private readonly ITodoItemStore todoItemStore;

        public DeleteTodoItemCommandHandler(ITodoItemStore todoItemStore)
        {
            this.todoItemStore = todoItemStore;
        }

        public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = await todoItemStore.GetAsync(request.Id);

            if (todoItem == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            await todoItemStore.DeleteAsync(todoItem, cancellationToken);

            return Unit.Value;
        }
    }
}
