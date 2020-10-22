using System;
using System.Collections.Generic;
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
            Mvx.IoCProvider.RegisterType<IToDoService, ToDoService>();

            RegisterAppStart<ToDoViewModel>();
        }
    }
}
