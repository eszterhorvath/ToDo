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
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadTodos();
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            await LoadTodos();
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

        public async Task LoadTodos()
        {
            var todos = await _todoService.GetTodosAsync();
            todos.Sort(new TodoComparer());
            ToDos = todos;
        }

        public async void RemoveTodo(Models.ToDo toDoItem)
        {
            ToDos.Remove(toDoItem);

            await _todoService.DeleteTodo(toDoItem);

            await LoadTodos();
        }

        public async void ChangeState(Models.ToDo toDoItem)
        {
            toDoItem.State = (toDoItem.State == State.Done) ? State.Pending : State.Done;

            await _todoService.SaveTodoAsync(toDoItem);

            await LoadTodos();
        }
    }
}
