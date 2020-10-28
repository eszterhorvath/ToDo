using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Views;
using ToDo.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditView : MvxContentPage<EditViewModel>
    {
        public EditView()
        {
            InitializeComponent();
        }
    }
}