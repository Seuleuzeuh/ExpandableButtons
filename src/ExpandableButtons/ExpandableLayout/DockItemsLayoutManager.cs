using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ExpandableButtons.Layouts;
using Xamarin.Forms;

namespace ExpandableButtons.ExpandableLayout
{
    internal class DockItemsLayoutManager : ItemsLayoutManager
    {
        private Grid _itemsContainer;
        private DockLayout _dockLayout;
        private readonly DockItemsLayout _dockItemsLayout;

        protected uint AnimationLength { get; } = 150;

        internal DockItemsLayoutManager(DockItemsLayout layout) : base(layout)
        {
            _dockItemsLayout = layout;
        }

        internal override View GenerateLayout(ObservableCollection<ButtonItem> items, ButtonItem mainButton, bool isOpen)
        {
            _itemsContainer = GenerateItems(items, isOpen);
            _dockLayout = new DockLayout();
            _dockLayout.Children.Add(_itemsContainer);
            _dockLayout.Children.Add(mainButton);
            return _dockLayout;
        }

        private Grid GenerateItems(ObservableCollection<ButtonItem> items, bool isOpen)
        {
            Grid itemsContainer = new Grid()
            {
                ColumnSpacing = _dockItemsLayout.ItemsSpacing,
                RowSpacing = _dockItemsLayout.ItemsSpacing,
                Margin = GetItemsContainerMargin(),
                IsVisible = isOpen,
                Opacity = isOpen ? 1 : 0
            };
            for (int i = 0; i < items.Count; i++)
            {
                ButtonItem item = items[i];
                if (_dockItemsLayout.Dock == Dock.Top || _dockItemsLayout.Dock == Dock.Bottom)
                    Grid.SetRow(item, i);
                else
                    Grid.SetColumn(item, i);

                itemsContainer.Children.Add(item);
            }

            itemsContainer.SetValue(DockLayout.DockProperty, _dockItemsLayout.Dock);
            return itemsContainer;
        }

        private Thickness GetItemsContainerMargin()
        {
            return _dockItemsLayout.Dock switch
            {
                Dock.Left => new Thickness(0, 0, _dockItemsLayout.ItemsSpacing, 0),
                Dock.Right => new Thickness(_dockItemsLayout.ItemsSpacing, 0, 0, 0),
                Dock.Bottom => new Thickness(0, _dockItemsLayout.ItemsSpacing, 0, 0),
                _ => new Thickness(0, 0, 0, _dockItemsLayout.ItemsSpacing),
            };
        }

        internal override void ItemsAdded(IEnumerable<ButtonItem> addedItems)
        {
            int index = _itemsContainer.Children.Count;
            foreach (var item in addedItems)
            {
                if (_dockItemsLayout.Dock == Dock.Top || _dockItemsLayout.Dock == Dock.Bottom)
                    Grid.SetRow(item, index);
                else
                    Grid.SetColumn(item, index);

                _itemsContainer.Children.Add(item);
                index++;
            }
        }

        internal override void ItemsRemoved(IEnumerable<ButtonItem> removedItems)
        {
            int totalItems = _itemsContainer.Children.Count;
            foreach (var item in removedItems)
            {
                int itemIndex = _itemsContainer.Children.IndexOf(item);

                if (itemIndex + 1 < totalItems)
                {
                    for (int i = itemIndex + 1; i < totalItems; i++)
                    {
                        if (_dockItemsLayout.Dock == Dock.Top || _dockItemsLayout.Dock == Dock.Bottom)
                            Grid.SetRow(item, i - 1);
                        else
                            Grid.SetColumn(item, i - 1);
                    }

                }

                _itemsContainer.Children.Remove(item);
                totalItems--;
            }
        }

        internal override Task OpenAsync()
        {
            if (_itemsContainer == null)
                return null;

            _itemsContainer.IsVisible = true;
            var tcs = new TaskCompletionSource<bool>();
            var appeaingAnimation = new Animation();
            appeaingAnimation.WithConcurrent(
                (f) => _itemsContainer.Opacity = f,
                0, 1);

            appeaingAnimation.Commit(_itemsContainer, nameof(OpenAsync), length: AnimationLength,
                finished: (v, t) => tcs.SetResult(true));

            return tcs.Task;
        }

        internal override Task CloseAsync()
        {
            if (_itemsContainer == null)
                return null;

            var tcs = new TaskCompletionSource<bool>();
            var disapperingAnimation = new Animation();
            disapperingAnimation.WithConcurrent(
                (f) => _itemsContainer.Opacity = f,
                1, 0);

            disapperingAnimation.Commit(_itemsContainer, nameof(CloseAsync), length: AnimationLength,
                finished: (v, t) =>
                {
                    tcs.SetResult(true);
                    _itemsContainer.IsVisible = false;
                });
            return tcs.Task;
        }

    }
}
