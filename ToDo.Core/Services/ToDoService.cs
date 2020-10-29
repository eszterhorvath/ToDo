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
            var c =_database.GetConnection();

            c.CreateTable<Models.ToDo>();

            c.Execute("CREATE VIRTUAL TABLE IF NOT EXISTS ToDo_VT " +
                      "USING FTS5(Id,Title,Description,State)");
            c.Execute("CREATE TRIGGER IF NOT EXISTS UpdateVTWhenNewItemWasInserted " +
                      "AFTER INSERT " +
                      "ON ToDo " +
                      "BEGIN " +
                      "INSERT INTO ToDo_VT VALUES (new.Id,new.Title,new.Description,new.State); " +
                      "END;");
            c.Execute("CREATE TRIGGER IF NOT EXISTS UpdateVTWhenItemWasModified " +
                      "AFTER UPDATE " +
                      "ON ToDo " +
                      "BEGIN " +
                          "DELETE FROM ToDo_VT WHERE Id = old.Id; " +
                          "INSERT INTO ToDo_VT VALUES (old.Id,new.Title,new.Description,new.State); " +
                      "END;");

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

            return c.Query<Models.ToDo>("SELECT * " +
                                         "FROM ToDo_VT " +
                                         "WHERE ToDo_VT MATCH ?", searchedText);
        }
    }

}
