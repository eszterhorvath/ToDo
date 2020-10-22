using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MvvmCross;
using MvvmCross.ViewModels;
using ToDo.Core.Services;
using ToDo.Core.ViewModels;

namespace ToDo.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Todos.db3");
            Mvx.IoCProvider.RegisterSingleton<IToDoService>(new ToDoService(dbPath));

            RegisterAppStart<ToDoViewModel>();
        }
    }
}
