using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Parse.StringDictionary;
using ToDo.Core.ViewModels;
using ToDo.Core;
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

        async void OnButtonClicked(object sender, EventArgs e)
        {
            var addPage = new AddView();

            await Navigation.PushModalAsync(addPage);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                bool sure = await DisplayAlert("Delete item", "Are you sure you want to delete this item?", "Yes",
                    "No");

                if (sure)
                {
                    ViewModel.RemoveTodo((Core.Models.ToDo) e.SelectedItem);
                }
                else
                {
                    ((ListView) sender).SelectedItem = null;
                }
            }
        }
    }
}