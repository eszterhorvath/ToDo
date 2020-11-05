using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ToDo.Core.Services
{
    public interface INativeViewService
    {
        int[] GetCoordinates(VisualElement visualElement);
        int ValidateYPosition(int y, VisualElement visualElement);
    }
}
