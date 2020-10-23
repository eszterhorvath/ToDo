using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;

namespace ToDo.Core.ViewModels
{
    public class ToDoViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;
        public ToDoViewModel(IToDoService todoService)
        {
            _todoService = todoService;

            LoadTodos();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            LoadTodos();
        }

        private List<Models.ToDo> _toDos;
        public List<Models.ToDo> ToDos
        {
            get => _toDos;
            set
            {
                _toDos = value;
                RaisePropertyChanged();
            }
        }

        public async void RemoveTodo(Models.ToDo toDoItem)
        {
            ToDos.Remove(toDoItem);

            await _todoService.DeleteTodo(toDoItem);

            await LoadTodos();
        }

        public async Task LoadTodos()
        {
            ToDos = await _todoService.GetTodosAsync();
            ToDos.Sort(new TodoComparer());
        }

        public async void ChangeState(Models.ToDo toDoItem)
        {
            Models.ToDo todo = await _todoService.GetTodoAsync(toDoItem.Id);

            todo.State = (todo.State == State.Done) ? State.Pending : State.Done;

            await _todoService.SaveTodoAsync(todo);

            await LoadTodos();
        }
    }
}
