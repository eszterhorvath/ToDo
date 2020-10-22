using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Parse.StringDictionary;
using MvvmCross.Forms.Views;
using ToDo.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddView : MvxContentPage<AddViewModel>
    {
        public AddView()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            ViewModel.AddTodo();

            await Navigation.PopModalAsync();
        }
    }
}