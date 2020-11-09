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
using Xamarin.Essentials;

namespace ToDo.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddView : MvxContentPage<AddViewModel>
    {
        public AddView()
        {
            InitializeComponent();
        }

        private async void CameraButton_OnClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            var stream = await photo.OpenReadAsync();

            PhotoImage.Source = ImageSource.FromStream(() => stream);
        }
    }
}