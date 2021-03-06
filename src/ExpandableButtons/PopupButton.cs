﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using ExpandableButtons.ExpandableLayout;
using ExpandableButtons.Helpers;
using Xamarin.Forms;

namespace ExpandableButtons
{
    [ContentProperty(nameof(Items))]
    public class PopupButton : TemplatedView
    {
        const string ContainerPart = "PART_Container";

        Layout<View> _container;
        bool _collectionChangedHandled;
        private ItemsLayoutManager _layoutManager;

        public PopupButton()
        {
            Items = new ObservableCollection<ButtonItem>();
            InternalSubItemCommand = new Command(() => IsOpen = false);
            UpdateIsEnabled();
        }
        
        public event EventHandler Opened;
        public event EventHandler Closed;

        private ICommand InternalSubItemCommand { get; }

        public ObservableCollection<ButtonItem> Items { get; set; }

        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<ButtonItem>), typeof(PopupButton), null,
               propertyChanged: OnItemsSourcePropertyChanged, defaultBindingMode: BindingMode.TwoWay);

        public IEnumerable<ButtonItem> ItemsSource
        {
            get { return (IEnumerable<ButtonItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PopupButton)?.UpdateItemsSource();
        }

        public static readonly BindableProperty ItemsLayoutProperty =
           BindableProperty.Create(nameof(ItemsLayout), typeof(IItemsLayout), typeof(PopupButton), new DockItemsLayout(),
               propertyChanged: OnItemsLayoutPropertyChanged);

        private static void OnItemsLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PopupButton)?.UpdateLayout();
        }

        public static readonly BindableProperty HorizontalContentOptionsProperty =
          BindableProperty.Create(nameof(HorizontalContentOptions), typeof(LayoutOptions), typeof(PopupButton), LayoutOptions.Fill,
              defaultBindingMode: BindingMode.OneWay);

        public LayoutOptions HorizontalContentOptions
        {
            get { return (LayoutOptions)GetValue(HorizontalContentOptionsProperty); }
            set { SetValue(HorizontalContentOptionsProperty, value); }
        }

        public IItemsLayout ItemsLayout
        {
            get { return (IItemsLayout)GetValue(ItemsLayoutProperty); }
            set { SetValue(ItemsLayoutProperty, value); }
        }

        public static readonly BindableProperty IsOpenProperty =
          BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(PopupButton), false,
              propertyChanged: OnIsOpenPropertyChanged, defaultBindingMode: BindingMode.TwoWay);

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        static void OnIsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PopupButton)?.UpdateIsOpen();
        }

        public static readonly BindableProperty ButtonProperty =
          BindableProperty.Create(nameof(Button), typeof(ButtonItem), typeof(PopupButton), null,
              propertyChanged: OnButtonPropertyChanged, defaultBindingMode: BindingMode.TwoWay);

        public ButtonItem Button
        {
            get { return (ButtonItem)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        private static void OnButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PopupButton)?.UpdateButton();
        }

        private void UpdateButton()
        {
            UpdateLayout();
        }

        private void UpdateIsOpen()
        {
            if (_layoutManager == null)
                return;

            if (IsOpen)
            {
                _layoutManager.OpenAsync().ExecuteWithoutAwait();
                Opened?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _layoutManager.CloseAsync().ExecuteWithoutAwait();
                Closed?.Invoke(this, EventArgs.Empty);
            }

            Button?.SetSelectedState(IsOpen);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ContainerPart) as Layout<View>;
        }

        void UpdateItemsSource()
        {
            Items.Clear();
            foreach (var item in ItemsSource)
                Items.Add(item);
        }

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                if (!_collectionChangedHandled)
                {
                    Items.CollectionChanged += OnItemsCollectionChanged;
                    _collectionChangedHandled = true;
                }

                foreach (var subItem in Items)
                {
                    subItem.IsEnabled = true;
                }
            }
            else
            {
                if (_collectionChangedHandled)
                {
                    Items.CollectionChanged -= OnItemsCollectionChanged;
                    _collectionChangedHandled = false;
                }

                foreach (var subItem in Items)
                {
                    subItem.IsEnabled = false;
                }
            }


            UpdateLayout();
        }

        void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_layoutManager == null)
                return;

            if (e.OldItems != null)
                _layoutManager.ItemsRemoved(e.OldItems.OfType<ButtonItem>());

            if (e.NewItems != null)
            {
                var addedItems = e.NewItems.OfType<ButtonItem>();
                _layoutManager.ItemsAdded(addedItems);
                SetSubItemInternalCommand(Items);
                UpdateButtonItemsBindingContext(addedItems);
            }
        }

        void UpdateLayout()
        {
            if (_container == null || Button == null || (_layoutManager != null && ItemsLayout == _layoutManager.Layout))
                return;

            _container.Children.Clear();
            if (ItemsLayout is DockItemsLayout dockLayout)
            {
                _layoutManager = new DockItemsLayoutManager(dockLayout);
            }

            if (_layoutManager != null)
            {
                View generatedLayout = _layoutManager.GenerateLayout(Items, Button, IsOpen);
                UpdateButtonItemsBindingContext(Items);
                _container.Children.Add(generatedLayout);
                SetSubItemInternalCommand(Items);
                Button.InternalCommand = new Command(() =>
                {
                    IsOpen = !IsOpen;
                });
            }
        }

        private void SetSubItemInternalCommand(IEnumerable<ButtonItem> items)
        {
            foreach (var item in items)
                item.InternalCommand = InternalSubItemCommand;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateButtonItemsBindingContext(Items);
        }

        private void UpdateButtonItemsBindingContext(IEnumerable<ButtonItem> buttonItems)
        {
            if (Items == null || Items.Count == 0)
                return;

            foreach (var buttonItem in buttonItems.ToList())
                UpdateButtonItemBindingContext(buttonItem);
        }

        private void UpdateButtonItemBindingContext(ButtonItem buttonItem)
        {
            if (buttonItem == null)
                return;

            buttonItem.BindingContext = BindingContext;
        }
    }
}
