using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Core.Parse.StringDictionary;
using ToDo.Core.ViewModels;
using ToDo.Core;
using ToDo.Core.Models;
using ToDo.Core.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoView : MvxContentPage<ToDoViewModel>
    {
        private readonly INativeViewService _nativeViewService;

        public ToDoView()
        {
            InitializeComponent();
            _nativeViewService = Mvx.IoCProvider.Resolve<INativeViewService>();
        }

        private void OnGridTapped(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            ViewModel.SelectedTodo = (Core.Models.ToDo)grid.BindingContext;

            DeleteTextGrid.HeightRequest = grid.Height;

            int y = _nativeViewService.GetYPosition(grid, (StackLayout)grid.Parent.Parent.Parent);

            // Ensure that the grid won't hang out from the screen
            y = _nativeViewService.GetLowestPossibleYPosition(y, grid);

            FrontSideToFlip.TranslationY = y;
            BackSideToFlip.TranslationY = y;

            FrontSide.IsVisible = true;
        }

        private async void OnRemoveTapped(object sender, EventArgs e)
        {
            await Flip(FrontSideToFlip, BackSideToFlip);
        }

        private async Task Flip(VisualElement from, VisualElement to)
        {
            await from.RotateYTo(-90, 300, Easing.SpringIn);
            to.RotationY = 90;
            BackSide.IsVisible = true;
            FrontSide.IsVisible = false;
            await to.RotateYTo(0, 300, Easing.SpringOut);
            from.RotationY = 0;
        }
    }
}
