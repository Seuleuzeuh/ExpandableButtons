using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ExpandableButtons.Helpers;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace ExpandableButtons
{
    public class ButtonItem : TemplatedView
    {
        const string BorderPart = "PART_Border";

        Frame _border;
        private TouchEffect _touchEff;

        public ButtonItem()
        {
            //TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            //tapGesture.Tapped += TapGestureTapped;
            //GestureRecognizers.Add(tapGesture);
            _touchEff = new TouchEffect();
            Effects.Add(_touchEff);
            _touchEff.StateChanged += TouchStateChanged;
            TouchEffect.SetCommand(this, new Command(InternalExecuteCommand));
        }

        private void InternalExecuteCommand()
        {
            Command?.Execute(CommandParameter);
        }

        private void TouchStateChanged(object sender, TouchStateChangedEventArgs args)
        {
            if(args.State == TouchState.Pressed)
            {

            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild(BorderPart) as Frame;
        }

        internal void SetSelectedState()
        {

        }

        public static readonly BindableProperty CommandProperty =
         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonItem), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
         BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonItem), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        //public static readonly BindableProperty ContentProperty =
        // BindableProperty.Create(nameof(Content), typeof(View), typeof(ButtonItem), null);

        //public View Content
        //{
        //    get { return (View)GetValue(ContentProperty); }
        //    set { SetValue(ContentProperty, value); }
        //}

        public static readonly BindableProperty TextProperty =
         BindableProperty.Create(nameof(Text), typeof(string), typeof(ButtonItem), null, propertyChanged: OnButtonItemPropertyChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextSelectedProperty =
         BindableProperty.Create(nameof(TextSelected), typeof(string), typeof(ButtonItem), null, propertyChanged: OnButtonItemPropertyChanged);

        public string TextSelected
        {
            get { return (string)GetValue(TextSelectedProperty); }
            set { SetValue(TextSelectedProperty, value); }
        }

        public static readonly BindableProperty ImageSourceProperty =
         BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ButtonItem), null, propertyChanged: OnButtonItemPropertyChanged);

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly BindableProperty ImageSourceSelectedProperty =
         BindableProperty.Create(nameof(ImageSourceSelected), typeof(ImageSource), typeof(ButtonItem), null, propertyChanged: OnButtonItemPropertyChanged);

        public ImageSource ImageSourceSelected
        {
            get { return (ImageSource)GetValue(ImageSourceSelectedProperty); }
            set { SetValue(ImageSourceSelectedProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
         BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(ButtonItem), float.Epsilon);

        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty ColorProperty =
         BindableProperty.Create(nameof(Color), typeof(Color), typeof(ButtonItem), Color.Transparent, propertyChanged: OnButtonItemPropertyChanged);

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty ColorSelectedProperty =
         BindableProperty.Create(nameof(ColorSelected), typeof(Color), typeof(ButtonItem), Color.Default, propertyChanged: OnButtonItemPropertyChanged);

        public Color ColorSelected
        {
            get { return (Color)GetValue(ColorSelectedProperty); }
            set { SetValue(ColorSelectedProperty, value); }
        }

        public static readonly BindableProperty ColorPressedProperty =
         BindableProperty.Create(nameof(ColorPressed), typeof(Color), typeof(ButtonItem), Color.Default);

        public Color ColorPressed
        {
            get { return (Color)GetValue(ColorPressedProperty); }
            set { SetValue(ColorPressedProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ButtonItem), Color.Default, propertyChanged: OnButtonItemPropertyChanged);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorSelectedProperty =
         BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(ButtonItem), Color.Default, propertyChanged: OnButtonItemPropertyChanged);

        public Color TextColorSelected
        {
            get { return (Color)GetValue(TextColorSelectedProperty); }
            set { SetValue(TextColorSelectedProperty, value); }
        }

        public static readonly BindableProperty TextColorPressedProperty =
        BindableProperty.Create(nameof(TextColorPressed), typeof(Color), typeof(ButtonItem), Color.Default, propertyChanged: OnButtonItemPropertyChanged);

        public Color TextColorPressed
        {
            get { return (Color)GetValue(TextColorPressedProperty); }
            set { SetValue(TextColorPressedProperty, value); }
        }

        private static void OnButtonItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ButtonItem)?.UpdateCurrent();
        }

        public static readonly BindableProperty IsSelectedProperty =
         BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ButtonItem), false, propertyChanged: OnButtonItemPropertyChanged);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty CurrentColorProperty =
         BindableProperty.Create(nameof(CurrentColor), typeof(Color), typeof(ButtonItem), Color.Transparent);

        public Color CurrentColor
        {
            get { return (Color)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        public static readonly BindableProperty CurrentImageSourceProperty =
         BindableProperty.Create(nameof(CurrentImageSource), typeof(ImageSource), typeof(ButtonItem), null);

        public ImageSource CurrentImageSource
        {
            get { return (ImageSource)GetValue(CurrentImageSourceProperty); }
            set { SetValue(CurrentImageSourceProperty, value); }
        }

        public static readonly BindableProperty CurrentTextProperty =
         BindableProperty.Create(nameof(CurrentText), typeof(string), typeof(ButtonItem), null, propertyChanged: OnButtonItemPropertyChanged);

        public string CurrentText
        {
            get { return (string)GetValue(CurrentTextProperty); }
            set { SetValue(CurrentTextProperty, value); }
        }

        public static readonly BindableProperty CurrentTextColorProperty =
         BindableProperty.Create(nameof(CurrentTextColor), typeof(Color), typeof(ButtonItem), Color.Default, propertyChanged: OnButtonItemPropertyChanged);

        public Color CurrentTextColor
        {
            get { return (Color)GetValue(CurrentTextColorProperty); }
            set { SetValue(CurrentTextColorProperty, value); }
        }

        private void UpdateCurrent()
        {
            CurrentColor = IsSelected && ColorSelected != Color.Default ? ColorSelected : Color;
            CurrentImageSource = IsSelected && ImageSourceSelected != null ? ImageSourceSelected : ImageSource;
            CurrentText = IsSelected && !string.IsNullOrEmpty(TextSelected) ? TextSelected : Text;
            CurrentTextColor = IsSelected && TextColorSelected != Color.Default ? TextColorSelected : TextColor;
        }
    }
}
