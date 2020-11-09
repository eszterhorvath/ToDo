using ToDo.Core.Services;
using ToDo.UI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToDo.Droid
{
    public class AndroidViewService : INativeViewService
    {
        public int GetYPosition(VisualElement visualElement, VisualElement parent)
        {
            // Get the position of the StackLayout
            var stackLayout = (StackLayout)parent;

            var view = Xamarin.Forms.Platform.Android.Platform.GetRenderer(stackLayout).View;

            var coords = new int[4];
            view.GetLocationOnScreen(coords);

            var density = view.Context.Resources.DisplayMetrics.Density;

            var stackLayoutY = (int)(coords[1] / density);

            // Get the position of the selected grid
            view = Xamarin.Forms.Platform.Android.Platform.GetRenderer(visualElement).View;

            view.GetLocationOnScreen(coords);

            var gridY = (int)(coords[1] / density);

            return gridY - stackLayoutY;
        }

        public int GetLowestPossibleYPosition(int y, VisualElement grid)
        {
            var height = (int)(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);

            if (y > (height - ((int)grid.Bounds.Height + 365)))
            {
                y = height - ((int)grid.Bounds.Height + 365);
            }

            return y;
        }
    }
}