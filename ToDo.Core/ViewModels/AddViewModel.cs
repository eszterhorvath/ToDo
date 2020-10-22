using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MvvmCross.ViewModels;
using ToDo.Core.Models;
using ToDo.Core.Services;

namespace ToDo.Core.ViewModels
{
    public class AddViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;

        public Models.ToDo ToDoItem { get; set; }

        public AddViewModel(IToDoService todoService)
        {
            _todoService = todoService;

            ToDoItem = new Models.ToDo();
        }

        public async void AddTodo()
        {
            if (ToDoItem.Title != null)
            {
                await _todoService.SaveTodoAsync(ToDoItem);
            }
        }
    }
}
