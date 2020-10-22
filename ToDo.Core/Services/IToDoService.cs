using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Core.Services
{
    public interface IToDoService
    {
        Task<List<Models.ToDo>> GetTodosAsync();
        Task<Models.ToDo> GetTodoAsync(int id);
        Task<int> SaveTodoAsync(Models.ToDo todo);
        Task<int> DeleteTodo(Models.ToDo todo);
    }
}
