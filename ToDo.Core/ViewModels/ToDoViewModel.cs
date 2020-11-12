using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bogus;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;
using ToDo.Core.Models;
using ToDo.Core.Services;
using Xamarin.Forms;

namespace ToDo.Core.ViewModels
{
    public class ToDoViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;
        private readonly IMvxNavigationService _navigationService;

        private readonly IDisposable _subscription;

        public ICommand AddTodoItemCommand { get; }
        public ICommand ChangeStateCommand { get; }
        public ICommand DeleteTodoItemCommand { get; }
        public ICommand EditTodoItemCommand { get; }
        public ICommand SearchTextChangedCommand { get; }
        public ICommand FadeBackgroundCloseCommand { get; }

        public ToDoViewModel(IToDoService todoService, IMvxNavigationService navigationService)
        {
            _todoService = todoService;
            _navigationService = navigationService;

            AddTodoItemCommand = new Command(
                async () => await AddNewTodo());
            ChangeStateCommand = new Command(
                async () => await ChangeState());
            DeleteTodoItemCommand = new Command(
                async () => await DeleteTodo());
            EditTodoItemCommand = new Command(
                async () => await EditTodo());
            SearchTextChangedCommand = new Command(
                async (query) => await SearchedTextChanged((string)query));
            FadeBackgroundCloseCommand = new Command(
                async () => await FadeBackgroundClose());

            _subscription = this.WhenAnyValue(x => x.SearchedString)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ObserveOn(RxApp.MainThreadScheduler)
                .SelectMany(x =>
                {
                    return SearchTodos(x).ContinueWith((a) => Task.FromResult(Unit.Default));
                })
                .Subscribe();
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadTodos();
        }

        public override async void ViewAppeared()
        {
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


        private string _searchedString;
        public string SearchedString
        {
            get => _searchedString;
            set
            {
                _searchedString = value;
                RaisePropertyChanged();
            }
        }


        private Models.ToDo _selectedTodo;
        public Models.ToDo SelectedTodo
        {
            get => _selectedTodo;
            set
            {
                _selectedTodo = value;
                ChangeStateText = _selectedTodo.State == State.Pending
                    ? State.Done.ToString()
                    : State.Pending.ToString();
                RaisePropertyChanged();
            }
        }


        private string _changeStateText;
        public string ChangeStateText
        {
            get => _changeStateText;
            set
            {
                _changeStateText = value;
                RaisePropertyChanged();
            }
        }


        private bool _fadeBackgroundFrontSideVisibility;
        public bool FadeBackgroundFrontSideVisibility
        {
            get => _fadeBackgroundFrontSideVisibility;
            set
            {
                _fadeBackgroundFrontSideVisibility = value;
                RaisePropertyChanged();
            }
        }


        private bool _fadeBackgroundBackSideVisibility;
        public bool FadeBackgroundBackSideVisibility
        {
            get => _fadeBackgroundBackSideVisibility;
            set
            {
                _fadeBackgroundBackSideVisibility = value;
                RaisePropertyChanged();
            }
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

        //internal async Task RemoveTodo()
        //{
            //var confirmed = await _userDialogService.ConfirmAsync(new ConfirmConfig()
            //{
            //    Message = "Are you sure you want to delete this item?",
            //    OkText = "Delete",
            //    CancelText = "Cancel"
            //});

            //if (confirmed)
            //{
            //    ToDos.Remove(SelectedTodo);

            //    await _todoService.DeleteTodo(SelectedTodo);
            //}
        //}

        internal async Task DeleteTodo()
        {
            ToDos.Remove(SelectedTodo);

            await _todoService.DeleteTodo(SelectedTodo);

            FadeBackgroundBackSideVisibility = false;
        }

        internal async Task EditTodo()
        {
            await _navigationService.Navigate<EditViewModel, Models.ToDo>(SelectedTodo);
            FadeBackgroundFrontSideVisibility = false;
        }

        internal async Task ChangeState()
        {
            if (SelectedTodo.State == State.Done)
            {
                SelectedTodo.State = State.Pending;
                ChangeStateText = State.Done.ToString();
            }
            else
            {
                SelectedTodo.State = State.Done;
                ChangeStateText = State.Pending.ToString();

            }

            await _todoService.SaveTodoAsync(SelectedTodo);
        }

        internal async Task SearchTodos(string searchedString)
        {
            var resultList = await _todoService.SearchTodo(searchedString);

            ToDos.Clear();
            foreach (var t in resultList)
                ToDos.Add(t);
        }

        internal async Task SearchedTextChanged(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                await LoadTodos();
                SearchedString = query;
                return;
            }

            SearchedString = query;
        }

        internal async Task FadeBackgroundClose()
        {
            FadeBackgroundFrontSideVisibility = false;
            FadeBackgroundBackSideVisibility = false;

            await LoadTodos();
        }

        private async Task GenerateRandomData()
        {
            int i = 0;
            while (i != 10000)
            {
                i++;
                var testTodo = new Faker<Models.ToDo>()
                    .RuleFor(t => t.Title, s => s.Lorem.Sentence())
                    .RuleFor(t => t.Description, s => s.Lorem.Paragraph())
                    .RuleFor(t => t.State, s => s.PickRandom<Models.State>());
                await _todoService.SaveTodoAsync(testTodo);
            }
        }
    }
}
