using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Stores;
using CleanArchitecture.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using DbTodoList = CleanArchitecture.Infrastructure.Persistence.Entities.TodoList;
using DbTodoItem = CleanArchitecture.Infrastructure.Persistence.Entities.TodoItem;

namespace CleanArchitecture.Infrastructure.Persistence.Stores
{
    public class TodoListStore : ITodoListStore
    {
        private readonly IGetDbConnection getConnection;
        private readonly IMapper mapper;

        public TodoListStore(IGetDbConnection getConnection, IMapper mapper)
        {
            this.getConnection = getConnection;
            this.mapper = mapper;
        }

        public async Task<TodoList> GetAsync(int id)
        {
            using IDbConnection connection = getConnection.Get();

            var toDoList = await connection.GetAsync<DbTodoList>(id);

            return mapper.Map<TodoList>(toDoList);
        }

        public async Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getConnection.Get();

            var dbTodoList = mapper.Map<DbTodoList>(todoList);

            await connection.UpdateAsync(dbTodoList);
        }

        public async Task<IEnumerable<TodoList>> GetByTitle(string title)
        {
            var query = @"SELECT * FROM TodoLists WHERE Title = @Title";

            using IDbConnection connection = getConnection.Get();

            IEnumerable<DbTodoList> dbTodoLists = await connection.QueryAsync<DbTodoList>(query, new {title});

            return mapper.Map<IEnumerable<TodoList>>(dbTodoLists);
        }

        public async Task Delete(TodoList todoList)
        {
            using IDbConnection connection = getConnection.Get();

            await connection.DeleteAsync(todoList);
        }

        public async Task<int> InsertAsync(TodoList todoList)
        {
            using IDbConnection connection = getConnection.Get();

            DbTodoList dbTodoList = mapper.Map<DbTodoList>(todoList);

            return await connection.InsertAsync(dbTodoList);
        }

        public async Task<IEnumerable<TodoList>> GetAllWithItemsAsync()
        {
            var query = "SELECT * FROM TodoLists; SELECT * FROM TodoItems;";
            using IDbConnection connection = getConnection.Get();
            using var multi = await connection.QueryMultipleAsync(query);
            
            IEnumerable<DbTodoList> toDoLists = multi.Read<DbTodoList>();
            IEnumerable<DbTodoItem> toDoItems = multi.Read<DbTodoItem>();

            foreach (var todoList in toDoLists)
            {
                todoList.Items = toDoItems.Where(todoItem => todoItem.TodoListId == todoList.Id);
            }

            return mapper.Map<IEnumerable<TodoList>>(toDoLists);
        }
    }
}