using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Parse.StringDictionary;
using ToDo.Core.ViewModels;
using ToDo.Core;
using ToDo.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoView : MvxContentPage<ToDoViewModel>
    {
        public ToDoView()
        {
            InitializeComponent();
        }
    }
}
