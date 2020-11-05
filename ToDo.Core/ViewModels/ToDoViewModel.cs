using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
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
        private readonly IUserDialogs _userDialogService;
        private readonly INativeViewService _nativeViewService;

        private readonly IDisposable _subscription;

        public ICommand AddTodoItemCommand { get; }
        public ICommand ChangeStateCommand { get; }
        public ICommand DeleteTodoItemCommand { get; }
        public ICommand RemoveTodoItemCommand { get; }
        public ICommand EditTodoItemCommand { get; }
        public ICommand SearchTextChangedCommand { get; }
        public ICommand GridTappedCommand { get; }
        public ICommand FadeBackgroundCloseCommand { get; }

        public ToDoViewModel(IToDoService todoService, IMvxNavigationService navigationService, IUserDialogs userDialogService, INativeViewService nativeViewService)
        {
            _todoService = todoService;
            _navigationService = navigationService;
            _userDialogService = userDialogService;
            _nativeViewService = nativeViewService;

            AddTodoItemCommand = new Command(
                async () => await AddNewTodo());
            ChangeStateCommand = new Command(
                async () => await ChangeState());
            RemoveTodoItemCommand = new Command(
                async (frontSide) => await RemoveTodo((VisualElement)frontSide));
            DeleteTodoItemCommand = new Command(
                async () => await DeleteTodo());
            EditTodoItemCommand = new Command(
                async () => await EditTodo());
            SearchTextChangedCommand = new Command(
                async (query) => await SearchedTextChanged((string)query));
            GridTappedCommand = new Command(
                (tappedItem) => GridTapped((Grid)tappedItem));
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

        ~ToDoViewModel()
        {
            _subscription.Dispose();
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


        private double _selectedTodoY;
        public double SelectedTodoY
        {
            get => _selectedTodoY;
            set
            {
                _selectedTodoY = value;
                RaisePropertyChanged();
            }
        }


        private double _selectedTodoGridHeight;
        public double SelectedTodoGridHeight
        {
            get => _selectedTodoGridHeight;
            set
            {
                _selectedTodoGridHeight = value;
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

        internal async Task RemoveTodo(VisualElement frontSide)
        {
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

            var frontSideToFlip = frontSide.FindByName<Grid>("FrontSideToFlip");

            var parent = (Grid)frontSide.Parent;
            var backSide = parent.FindByName<Grid>("BackSide");
            var backSideToFlip = frontSide.FindByName<Grid>("BackSideToFlip");

            await Flip(frontSideToFlip, backSideToFlip);

        }

        internal async Task Flip(VisualElement from, VisualElement to)
        {
            await from.RotateYTo(-90, 300, Easing.SpringIn);
            to.RotationY = 90;
            FadeBackgroundBackSideVisibility = true;
            FadeBackgroundFrontSideVisibility = false;
            from.RotationY = 0;
            await to.RotateYTo(0, 300, Easing.SpringOut);
        }

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

        internal void GridTapped(Grid grid)
        {
            SelectedTodoGridHeight = grid.Height;

            var lw = (ListView)grid.Parent.Parent;

            var toDoItem = (Models.ToDo)grid.BindingContext;
            SelectedTodo = toDoItem;

            int[] bounds = _nativeViewService.GetCoordinates(grid);

            // Ensure that the grid won't hang out from the screen
            SelectedTodoY = _nativeViewService.ValidateYPosition(bounds[1], grid);

            FadeBackgroundFrontSideVisibility = true;
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
