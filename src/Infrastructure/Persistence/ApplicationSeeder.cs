using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Identity.Models;
using CleanArchitecture.Infrastructure.Persistence.Database;
using CleanArchitecture.Infrastructure.Persistence.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public static class ApplicationSeeder
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            Task<ApplicationUser> userWithUsername = userManager.FindByNameAsync(defaultUser.UserName);

            if (userWithUsername == null)
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }

        public static async Task SeedSampleDataAsync(ConnectionString connectionString)
        {
            using IDbConnection connection = new SqlConnection(connectionString.Value);

            IEnumerable<TodoList> todoLists = await connection.QueryAsync<TodoList>("SELECT * FROM TodoLists");
            // Seed, if necessary
            if (!todoLists.Any())
            {
                int listId = await connection.InsertAsync(new TodoList
                {
                    Title = "Shopping",
                    Created = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid()
                });

                await connection.ExecuteAsync(
                    "INSERT INTO TodoItems ( Title, Done, TodoListId ) VALUES ( @Title, @Done, @TodoListId)",
                    new List<TodoItem>
                    {
                        new TodoItem {Title = "Apples", Done = true, TodoListId = listId },
                        new TodoItem {Title = "Milk", Done = true, TodoListId = listId },
                        new TodoItem {Title = "Bread", Done = true, TodoListId = listId },
                        new TodoItem {Title = "Toilet paper", Done = false, TodoListId = listId },
                        new TodoItem {Title = "Pasta", Done = false, TodoListId = listId },
                        new TodoItem {Title = "Tissues", Done = false, TodoListId = listId },
                        new TodoItem {Title = "Tuna", Done = false, TodoListId = listId },
                        new TodoItem {Title = "Water", Done = false, TodoListId = listId }
                    });
            }
        }
    }
}