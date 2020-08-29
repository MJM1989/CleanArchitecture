using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoItems.Commands.UpdateTodoItemDetail
{
    public class UpdateTodoItemDetailCommand : IRequest
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public PriorityLevel Priority { get; set; }

        public string Note { get; set; }
    }

    public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
    {
        private readonly ITodoItemStore todoItemStore;

        public UpdateTodoItemDetailCommandHandler(ITodoItemStore todoItemStore)
        {
            this.todoItemStore = todoItemStore;
        }

        public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            TodoItem todoItem = await todoItemStore.GetAsync(request.Id);

            if (todoItem == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            todoItem.ListId = request.ListId;
            todoItem.Priority = request.Priority;
            todoItem.Note = request.Note;

            await todoItemStore.UpdateAsync(todoItem, cancellationToken);

            return Unit.Value;
        }
    }
}
