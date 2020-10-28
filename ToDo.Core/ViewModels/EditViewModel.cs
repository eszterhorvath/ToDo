using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;
using Xamarin.Forms;

namespace ToDo.Core.ViewModels
{
    public class EditViewModel : MvxViewModel<Models.ToDo>
    {
        private readonly IToDoService _todoService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogService;

        public ICommand SaveCommand { get; }

        public EditViewModel(IToDoService todoService, IMvxNavigationService navigationService, IUserDialogs userDialogService)
        {
            _todoService = todoService;
            _navigationService = navigationService;
            _userDialogService = userDialogService;

            SaveCommand = new Command(async () => await SaveTodoAsync());
        }

        public Models.ToDo ToDoItem { get; set; }

        public string ToDoItemState
        {
            get => ToDoItem.State.ToString();
            set
            {
                switch (value)
                {
                    case "Pending":
                        ToDoItem.State = State.Pending;
                        break;
                    case "Done":
                        ToDoItem.State = State.Done;
                        break;
                }
            }
        }

        public override void Prepare(Models.ToDo todo)
        {
            base.Prepare();
            ToDoItem = todo;
        }

        internal async Task SaveTodoAsync()
        {
            if (ToDoItem.Title == "")
            {
                _userDialogService.Alert("Title cannot be empty!");
                return;
            }

            await _todoService.SaveTodoAsync(ToDoItem);

            await _navigationService.Close(this);
        }
    }
}
