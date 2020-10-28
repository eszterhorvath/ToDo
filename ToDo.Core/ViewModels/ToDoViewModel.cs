using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
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
        private readonly IUserDialogs _userDialogService;

        public ICommand AddTodoItemCommand { get; }
        public ICommand ChangeStateCommand { get; }
        public ICommand RemoveTodoItemCommand { get; }
        public ICommand EditTodoItemCommand { get; }
        public ICommand SearchTextChangedCommand { get; }

        public ToDoViewModel(IToDoService todoService, IMvxNavigationService navigationService, IUserDialogs userDialogService)
        {
            _todoService = todoService;
            _navigationService = navigationService;
            _userDialogService = userDialogService;

            AddTodoItemCommand = new Command(
                async () => await AddNewTodo());
            ChangeStateCommand = new Command(
                async (selectedItem) => await ChangeState((Models.ToDo)selectedItem));
            RemoveTodoItemCommand = new Command(
                async (swipedItem) => await RemoveTodo((Models.ToDo)swipedItem));
            EditTodoItemCommand = new Command(
                async (swipedItem) => await EditTodo((Models.ToDo)swipedItem));
            SearchTextChangedCommand = new Command(
                async (query) => await SearchedTextChanged((string)query));
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

        public string SearchedString { get; set; }


        private Models.ToDo _selectedTodo;
        public Models.ToDo SelectedTodo
        {
            get => _selectedTodo;
            set => _selectedTodo = null;
        }

        private async Task LoadTodos()
        {
            var todosList = await _todoService.GetTodosAsync();
            todosList.Sort(new TodoComparer());

            var todosCollection = new ObservableCollection<Models.ToDo>(todosList);
            ToDos = todosCollection;
        }

        internal async Task AddNewTodo()
        {
            await _navigationService.Navigate<AddViewModel>();
        }

        internal async Task RemoveTodo(Models.ToDo toDoItem)
        {
            var confirmed = await _userDialogService.ConfirmAsync(new ConfirmConfig()
            {
                Message = "Are you sure you want to delete this item?",
                OkText = "Delete",
                CancelText = "Cancel"
            });

            if (confirmed)
            {
                ToDos.Remove(toDoItem);

                await _todoService.DeleteTodo(toDoItem);

                await LoadTodos();
            }
        }

        internal async Task EditTodo(Models.ToDo toDoItem)
        {
            await _navigationService.Navigate<EditViewModel, Models.ToDo>(toDoItem);
        }

        internal async Task ChangeState(Models.ToDo toDoItem)
        {
            toDoItem.State = (toDoItem.State == State.Done) ? State.Pending : State.Done;

            await _todoService.SaveTodoAsync(toDoItem);

            await LoadTodos();
        }

        internal async Task SearchTodos()
        {
            var todoList = await _todoService.GetTodosAsync();

            var resultList = new List<Models.ToDo>();
            foreach (var todo in todoList)
            {
                if (todo.Title.ToLower().Contains(SearchedString))
                {
                    resultList.Add(todo);
                }
                else if (!String.IsNullOrWhiteSpace(todo.Description) && todo.Description.ToLower().Contains(SearchedString))
                {
                    resultList.Add(todo);
                }
            }

            var resultCollection = new ObservableCollection<Models.ToDo>(resultList);

            ToDos = resultCollection;
        }

        internal async Task SearchedTextChanged(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                await LoadTodos();
                SearchedString = query;
                return;
            }

            SearchedString = query.ToLower();
            
            await SearchTodos();
        }
    }
}
