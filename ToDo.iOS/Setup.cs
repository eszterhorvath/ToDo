using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.ViewModels;
using ToDo.Core;
using ToDo.Core.Services;
using ToDo.UI;
using UIKit;

namespace ToDo.iOS
{
    public class Setup : MvxFormsIosSetup<App, FormsApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<INativeViewService, iOSViewService>();
        }
    }
}