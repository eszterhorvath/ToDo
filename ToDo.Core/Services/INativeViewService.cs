using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ToDo.Core.Services
{
    public interface INativeViewService
    {
        int GetAdjustedYPosition(VisualElement visualElement, VisualElement parent);
        int GetLowestPossibleYPosition(int y, VisualElement visualElement);
    }
}
