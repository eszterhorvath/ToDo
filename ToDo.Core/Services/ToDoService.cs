using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ToDo.Core.Services
{
    public class ToDoService : IToDoService
    {
        readonly SQLiteAsyncConnection _database;

        public ToDoService(string dbPath){
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Models.ToDo>().Wait();
        }

        public Task<List<Models.ToDo>> GetTodosAsync()
        {
            return _database.Table<Models.ToDo>().ToListAsync();
        }

        public Task<Models.ToDo> GetTodoAsync(int id)
        {
            return _database.Table<Models.ToDo>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveTodoAsync(Models.ToDo todo)
        {
            if (todo.Id != 0)
            {
                return _database.UpdateAsync(todo);
            }
            else
            {
                return _database.InsertAsync(todo);
            }
        }

        public Task<int> DeleteTodo(Models.ToDo todo)
        {
            return _database.DeleteAsync(todo);
        }
    }
}
