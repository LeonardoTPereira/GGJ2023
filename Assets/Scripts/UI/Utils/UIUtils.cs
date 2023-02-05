using UnityEngine.UIElements;

namespace UI.Utils
{
    public static class UIUtils
    {
        public static void Display(this VisualElement panel, bool finalState)
        {
            if (panel == null) return;
            panel.style.display = finalState ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
