using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using ToDo.Core.Services;
using Xamarin.Forms;

namespace ToDo.UWP
{
    public class UWPViewService : INativeViewService
    {
        public int[] GetCoordinates(VisualElement visualElement)
        {
            // TODO

            int[] coords = {0, 0};

            return coords;
        }

        public int ValidateYPosition(int y, VisualElement grid)
        {
            // TODO

            return 0;
        }
    }
}
