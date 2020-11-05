using ToDo.Core.Services;
using ToDo.UI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToDo.Droid
{
    public class AndroidViewService : INativeViewService
    {
        public int[] GetCoordinates(VisualElement visualElement)
        {
            // Get the position of the StackLayout
            var stackLayout = (StackLayout)visualElement.Parent.Parent.Parent;

            var view = Xamarin.Forms.Platform.Android.Platform.GetRenderer(stackLayout).View;

            var coords = new int[4];
            view.GetLocationOnScreen(coords);

            var density = view.Context.Resources.DisplayMetrics.Density;

            var stackLayoutY = (int)(coords[1] / density);

            // Get the position of the selected grid
            view = Xamarin.Forms.Platform.Android.Platform.GetRenderer(visualElement).View;

            view.GetLocationOnScreen(coords);

            for (int i = 0; i < 4; i++)
            {
                coords[i] = (int)(coords[i] / density);
            }

            coords[1] = coords[1] - stackLayoutY;

            return coords;
        }

        public int ValidateYPosition(int y, VisualElement grid)
        {
            var height = (int)(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);

            /*
            // 1. Parent : ViewCell
            // 2. Parent : ListView
            // 3. Parent : StackLayout
            var stackLayout = (StackLayout)grid.Parent.Parent.Parent;

            // 20 : margin (10 above + 10 below)
            var buttonHeight = (int)stackLayout.Children[2].Bounds.Height + 20;
            */

            // Edit/Delete row : 60
            // Done/Pending row : 60
            // Close button row : 140
            // +5 to add a little space between the Close icon and the bottom of the screen
            // Sum : 265
            if (y > (height - ((int)grid.Bounds.Height + 365)))
            {
                y = height - ((int)grid.Bounds.Height + 365);
            }

            return y;
        }
    }
}