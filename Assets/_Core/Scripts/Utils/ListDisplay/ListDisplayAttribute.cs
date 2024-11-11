using UnityEngine;

namespace Utils.Drawer.ListDisplay
{
    public class ListDisplayAttribute : PropertyAttribute
    {
        public readonly DisplayMode mode;

        public ListDisplayAttribute(DisplayMode displayMode)
        {
            this.mode = displayMode;
        }

        public enum DisplayMode
        {
            Inline
        }
    }
}