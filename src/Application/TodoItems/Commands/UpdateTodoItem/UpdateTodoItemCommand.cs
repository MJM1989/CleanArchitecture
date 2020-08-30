using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItem
{
    public partial class UpdateTodoItemCommand : IRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
    {
        private readonly ITodoItemStore todoItemStore;

        public UpdateTodoItemCommandHandler(ITodoItemStore todoItemStore)
        {
            this.todoItemStore = todoItemStore;
        }

        public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = await todoItemStore.GetAsync(request.Id);

            if (todoItem == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            todoItem.Title = request.Title;
            todoItem.Done = request.Done;

            await todoItemStore.UpdateAsync(todoItem, cancellationToken);

            return Unit.Value;
        }
    }
}
