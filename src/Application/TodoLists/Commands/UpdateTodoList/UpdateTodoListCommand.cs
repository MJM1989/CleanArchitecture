using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList
{
    public class UpdateTodoListCommand : IRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
    {
        private readonly ITodoListStore todoListStore;

        public UpdateTodoListCommandHandler(ITodoListStore todoListStore)
        {
            this.todoListStore = todoListStore;
        }

        public async Task<Unit> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            TodoList entity = await todoListStore.GetAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            entity.Title = request.Title;

            await todoListStore.UpdateAsync(entity, cancellationToken);

            return Unit.Value;
        }
    }
}
