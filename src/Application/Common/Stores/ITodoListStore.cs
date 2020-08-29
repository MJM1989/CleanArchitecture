using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Stores
{
    public interface ITodoListStore
    {
        Task<TodoList> GetAsync(int id);
        Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken);
        Task<IEnumerable<TodoList>> GetByTitle(string title);
        Task Delete(TodoList todoList);
        Task<int> InsertAsync(TodoList todoList);
        Task<IEnumerable<TodoList>> GetAllWithItemsAsync();
    }
}