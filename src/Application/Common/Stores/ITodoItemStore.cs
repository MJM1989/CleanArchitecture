using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Stores
{
    public interface ITodoItemStore
    {
        Task<IEnumerable<TodoItem>> GetByListId(int listId);
        Task<TodoItem> GetAsync(int itemId);
        Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken);
        Task DeleteAsync(TodoItem todoItem, CancellationToken cancellationToken);
        Task<int> InsertAsync(TodoItem todoItem, CancellationToken cancellationToken);
    }
}