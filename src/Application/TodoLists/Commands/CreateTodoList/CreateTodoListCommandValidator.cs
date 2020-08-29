using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Stores;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        private readonly ITodoListStore todoListStore;

        public CreateTodoListCommandValidator(ITodoListStore todoListStore)
        {
            this.todoListStore = todoListStore;

            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            IEnumerable<TodoList> lists = await todoListStore.GetByTitle(title); 
            
            return !lists.Any();
        }
    }
}
