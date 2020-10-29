using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ToDo.Core.Services
{
    public class ToDoService : IToDoService
    {
        private readonly string _dbPath;
        SQLiteAsyncConnection _database;

        public ToDoService(string dbPath)
        {
            _dbPath = dbPath;
        }

        private Task EnsureInitialized()
        {
            if (_database != null)
                return Task.CompletedTask;

            _database = new SQLiteAsyncConnection(_dbPath);
            _database.GetConnection().CreateTable<Models.ToDo>();

            return Task.CompletedTask;
        }

        public async Task<List<Models.ToDo>> GetTodosAsync()
        {
            await EnsureInitialized();
            var c = _database.GetConnection();

            return c.Table<Models.ToDo>().ToList();
        }

        public async Task<Models.ToDo> GetTodoAsync(int id)
        {
            await EnsureInitialized();
            var c = _database.GetConnection();

            return c.Find<Models.ToDo>(x => x.Id == id);
        }

        public async Task<int> SaveTodoAsync(Models.ToDo todo)
        {
            await EnsureInitialized();
            var c = _database.GetConnection();

            if (todo.Id != 0)
            {
                return c.Update(todo);
            }
            else
            {
                return c.Insert(todo);
            }
        }

        public async Task<int> DeleteTodo(Models.ToDo todo)
        {
            await EnsureInitialized();
            var c = _database.GetConnection();

            return c.Delete(todo);
        }

        public async Task<List<Models.ToDo>> SearchTodo(string searchedText)
        {
            await EnsureInitialized();
            var c = _database.GetConnection();

            return c.Table<Models.ToDo>().Where(x => 
                (x.Title.ToLower().Contains(searchedText) || x.Description.ToLower().Contains(searchedText))).ToList();
        }
    }
}
