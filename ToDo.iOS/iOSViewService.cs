using CoreGraphics;
using ToDo.Core.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ToDo.iOS
{
    public class iOSViewService : INativeViewService
    {
        public int GetYPosition(VisualElement visualElement, VisualElement parent)
        {
            // Get the position of the StackLayout
            var stackLayout = (StackLayout)parent;

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

            var gridY = (int)coords.Y * -1;

            return gridY - stackLayoutY;
        }

        public int GetLowestPossibleYPosition(int y, VisualElement grid)
        {
            var height = (int)(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);

            if (y > (height - ((int)grid.Bounds.Height + 300)))
            {
                y = height - ((int)grid.Bounds.Height + 300);
            }

            return y;
        }
    }
}