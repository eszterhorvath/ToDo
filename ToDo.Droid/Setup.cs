using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using ToDo.Core.Services;
using ToDo.UI;

namespace ToDo.Droid
{
    public class Setup : MvxFormsAndroidSetup<Core.App, FormsApp>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<INativeViewService, AndroidViewService>();
        }
    }
}