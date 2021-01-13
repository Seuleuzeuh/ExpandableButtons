using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpandableButtons.ExpandableLayout
{
    internal abstract class ItemsLayoutManager
    {
        internal IItemsLayout Layout { get; }

        protected ItemsLayoutManager(IItemsLayout layout)
        {
            Layout = layout;
        }

        internal abstract View GenerateLayout(ObservableCollection<ButtonItem> items, ButtonItem mainButton, bool isOpen);

        internal abstract void ItemsAdded(IEnumerable<ButtonItem> addedItems);

        internal abstract void ItemsRemoved(IEnumerable<ButtonItem> removedItems);

        internal abstract Task OpenAsync();

        internal abstract Task CloseAsync();
    }
}
