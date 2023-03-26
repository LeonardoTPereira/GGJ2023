using System.Diagnostics;
using UnityEngine.UIElements;

namespace UI.Utils
{
    public static class UIUtils
    {
        public static bool Display(this VisualElement panel, bool finalState)
        {
            if (panel == null) 
                return false;

            panel.style.display = finalState ? DisplayStyle.Flex : DisplayStyle.None;
            return true;
        }
    }
}
