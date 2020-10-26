using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;
using Xamarin.Forms;

namespace ToDo.Core.ViewModels
{
    public class ToDoViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;
        private readonly IMvxNavigationService _navigationService;

        public ICommand AddTodoItemCommand { get; }

        public ToDoViewModel(IToDoService todoService, IMvxNavigationService navigationService)
        {
            _todoService = todoService;
            _navigationService = navigationService;

            AddTodoItemCommand = new Command(
                async () => { await _navigationService.Navigate<AddViewModel>();});
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

        private ObservableCollection<Models.ToDo> _toDos;
        public ObservableCollection<Models.ToDo> ToDos
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
            var todosList = await _todoService.GetTodosAsync();
            todosList.Sort(new TodoComparer());

            var todosCollection = new ObservableCollection<Models.ToDo>(todosList);
            ToDos = todosCollection;
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
