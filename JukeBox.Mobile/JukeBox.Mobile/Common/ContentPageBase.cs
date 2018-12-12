using Xamarin.Forms;
using GalaSoft.MvvmLight.Messaging;
using JukeBox.Mobile.ViewModels;

namespace JukeBox.Mobile.Common
{
    public abstract class ContentPageBase : ContentPage, IContentPageBase
    {
        public bool CancelsTouchesInView = true;

        public ContentPageBase()
        {
            NavigationPage.SetHasNavigationBar(this, true);
        }

        public void OnPreAppearing(bool clearData)
        {
            (BindingContext as INavigable)?.PreActivate(clearData);
        }

        protected override void OnAppearing()
        {
            if (BindingContext is ViewModelBase viewModel && !viewModel.IsActive)
            {
                viewModel.Activate(this.GetNavigationArgs());
                viewModel.IsActive = true;
            }
            else if (App.IsPausing)
            {
                App.IsPausing = false;
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is ViewModelBase viewModel && !App.IsPausing)
            {
                viewModel.Deactivate();
                viewModel.IsActive = false;
            }

            Messenger.Default.Unregister(this);
            base.OnDisappearing();
        }
    }
}
