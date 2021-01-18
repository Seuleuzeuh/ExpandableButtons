using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpandableButtons.Sample.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        public ICommand CameraCommand { get; }

        private void PopupButton_Opened(object sender, EventArgs e)
        {
            PopupButtonStateLabel.Text = "PopupButton is Opened";
        }

        private void PopupButton_Closed(object sender, EventArgs e)
        {
            PopupButtonStateLabel.Text = "PopupButton is Closed";
        }
    }
}