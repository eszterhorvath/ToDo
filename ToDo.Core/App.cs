﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Acr.UserDialogs;
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
            Mvx.IoCProvider.RegisterSingleton<IComputerVisionService>(new ComputerVisionService());
            Mvx.IoCProvider.RegisterSingleton<IMediaService>(new MediaService());
            Mvx.IoCProvider.RegisterSingleton(UserDialogs.Instance);

            RegisterAppStart<ToDoViewModel>();
        }
    }
}
