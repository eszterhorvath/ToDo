using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.ViewModels;
using ToDo.Core.Services;

namespace ToDo.Core.ViewModels
{
    public class ToDoViewModel : MvxViewModel
    {
        private readonly IToDoService _todoService;
        public ToDoViewModel(IToDoService todoService)
        {
            _todoService = todoService;

            _toDos = _todoService.LoadEntries();
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

        public void AddTodo(string title, string description)
        {
            var entry = new Models.ToDo()
            {
                Title = title,
                Description = description
            };

            ToDos.Add(entry);

            _todoService.AddEntry(title, description);
        }

        public void RemoveTodo(string title)
        {
            ToDos.Remove(
                ToDos.Find(x => x.Title.Equals(title))
                );

            _todoService.RemoveEntry(title);
        }
    }
}
