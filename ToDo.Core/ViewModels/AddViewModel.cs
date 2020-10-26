﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;
using Xamarin.Forms;

namespace ToDo.Core.ViewModels
{
    public class AddViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;
        private readonly IMvxNavigationService _navigationService;

        public Models.ToDo ToDoItem { get; set; }
        public ICommand AddTodoItemCommand { get; }

        public AddViewModel(IToDoService todoService, IMvxNavigationService navigationService)
        {
            _todoService = todoService;
            _navigationService = navigationService;

            ToDoItem = new Models.ToDo();
            AddTodoItemCommand = new Command(async () => await AddTodo());
        }

        public async Task AddTodo()
        {
            if (ToDoItem.Title != null)
            {
                ToDoItem.State = State.Pending;
                await _todoService.SaveTodoAsync(ToDoItem);

                await _navigationService.Close(this);
            }
        }
    }
}
