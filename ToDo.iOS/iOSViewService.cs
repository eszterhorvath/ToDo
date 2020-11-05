using CoreGraphics;
using ToDo.Core.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToDo.iOS
{
    public class iOSViewService : INativeViewService
    {
        public int[] GetCoordinates(VisualElement visualElement)
        {
            // Get the position of the StackLayout
            var stackLayout = (StackLayout)visualElement.Parent.Parent.Parent;

            var view = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(stackLayout).NativeView;

            var bounds = stackLayout.Bounds;
            CGPoint p = new CGPoint(bounds.X, bounds.Y);

            var coords = view.ConvertPointFromView(p, null);

            var stackLayoutY = (int)coords.Y * -1;

            // Get the position of the selected grid

            view = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(visualElement).NativeView;

            bounds = visualElement.Bounds;
            p = new CGPoint(bounds.X, bounds.Y);

            coords = view.ConvertPointFromView(p, null);

            var coordsArray = new int[] { (int)coords.X, (int)coords.Y * -1 };

            coordsArray[1] = coordsArray[1] - stackLayoutY;

            return coordsArray;
        }

        public double GetScreenHeight()
        {
            return DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        }

        public int ValidateYPosition(int y, VisualElement grid)
        {
            var height = (int)(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);

            // Edit/Delete row : 60
            // Done/Pending row : 60
            // Close button row : 140
            // +5 to add a little space between the Close icon and the bottom of the screen
            // Sum : 260
            if (y > (height - ((int)grid.Bounds.Height + 300)))
            {
                y = height - ((int)grid.Bounds.Height + 300);
            }

            return y;
        }

    }
}