using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace ExpandableButtons.Sample.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        public AboutViewModel()
        {
            IsTrucVisible = true;
            CameraCommand = new Command(() =>
            {
                App.Current.MainPage.DisplayPromptAsync("Camera Command", "You tapped on the Camera Button");
                IsTrucVisible = !IsTrucVisible;
            });
            CommandNotExecutable = new Command(() => { }, () => false);
        }

        public ICommand CameraCommand { get; }
        public ICommand CommandNotExecutable { get; }
        private bool _isTrucVisible;
        public bool IsTrucVisible
        {
            get { return _isTrucVisible; }
            set { SetProperty(ref _isTrucVisible, value); }
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                OnNotifyPropertyChanged(propertyName);
            }
        }

        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
        } 
        #endregion
    }
}
