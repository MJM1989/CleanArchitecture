using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Stores;
using CleanArchitecture.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using DbTodoItem = CleanArchitecture.Infrastructure.Persistence.Entities.TodoItem;

namespace CleanArchitecture.Infrastructure.Persistence.Stores
{
    public class TodoItemStore : ITodoItemStore
    {
        private readonly IGetDbConnection getConnection;
        private readonly IMapper mapper;

        public TodoItemStore(IGetDbConnection getConnection, IMapper mapper)
        {
            this.getConnection = getConnection;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TodoItem>> GetByListId(int listId)
        {
            var sql = "SELECT * FROM TodoItems WHERE ListId = @ListId";

            using IDbConnection connection = getConnection.Get();

            IEnumerable<DbTodoItem> dbItems = await connection.QueryAsync<DbTodoItem>(sql, new {listId});

            return mapper.Map<IEnumerable<TodoItem>>(dbItems);
        }

        public async Task<TodoItem> GetAsync(int itemId)
        {
            using IDbConnection connection = getConnection.Get();

            DbTodoItem item = await connection.GetAsync<DbTodoItem>(itemId);

            return mapper.Map<TodoItem>(item);
        }

        public async Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getConnection.Get();

            var dbTodoItem = mapper.Map<DbTodoItem>(todoItem);

            await connection.UpdateAsync(dbTodoItem);
        }

        public async Task DeleteAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getConnection.Get();

            await connection.DeleteAsync(todoItem);
        }

        public async Task<int> InsertAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getConnection.Get();

            DbTodoItem dbTodoItem = mapper.Map<DbTodoItem>(todoItem);

            return await connection.InsertAsync(dbTodoItem);
        }
    }
}