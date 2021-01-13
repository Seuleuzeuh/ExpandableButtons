using ExpandableButtons.Layouts;

namespace ExpandableButtons
{
    public class DockItemsLayout : IItemsLayout
    {
        public Dock Dock { get; set; } = Dock.Top;
        public double ItemsSpacing { get; set; } = 10;
    }
}
