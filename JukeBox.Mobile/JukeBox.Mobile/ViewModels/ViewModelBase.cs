using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Xamarin.Forms;
using System.Threading.Tasks;
using Acr.UserDialogs;
using JukeBox.Mobile.Services;
using JukeBox.Mobile.Common;

namespace JukeBox.Mobile.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, INavigable
    {
        protected IUserDialogs DialogService { get; }
        protected IAzureService AzureService { get; }
        protected INetworkService NetworkService { get; }

        protected IJukeBoxService JukeBoxService { get; }

        protected ViewModelBase()
        {
            DialogService = SimpleIoc.Default.GetInstance<IUserDialogs>();
            AzureService = SimpleIoc.Default.GetInstance<IAzureService>();
            NetworkService = SimpleIoc.Default.GetInstance<INetworkService>();
            JukeBoxService = SimpleIoc.Default.GetInstance<IJukeBoxService>();

            IsConnected = NetworkService.IsConnected;
            NetworkService.ConnectivityChanged += (s, e) =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => IsConnected = NetworkService.IsConnected);
            };

            OnNetworkAvailabilityChanged();
        }

        #region Properties
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (SetBusy(value) && !isBusy)
                {
                    BusyMessage = null;
                }
            }
        }

        private string busyMessage;
        public string BusyMessage
        {
            get { return busyMessage; }
            set { Set(ref busyMessage, value, broadcast: true); }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (Set(ref isConnected, value, broadcast: true))
                {
                    OnNetworkAvailabilityChanged();
                }
            }
        }
        #endregion

        public bool SetBusy(bool value, string message = null)
        {
            BusyMessage = message;

            var isSet = Set(() => IsBusy, ref isBusy, value, broadcast: true);
            if (isSet)
            {
                OnIsBusyChanged();
            }

            return isSet;
        }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => Set(ref isActive, value);
        }

        protected virtual void OnIsBusyChanged()
        {
        }

        public virtual void PreActivate(bool clearData)
        {
        }

        public virtual void Activate(object parameter)
        {
        }

        public virtual void Deactivate()
        {
        }

        protected virtual async void OnNetworkAvailabilityChanged()
        {
            if (!IsConnected && IsActive)
            {
                await DialogService.AlertAsync("Connection Unavailable");
            }
        }

        public virtual Task ShowErrorMessageAsync(Exception ex)
        {
#if DEBUG
            return ShowErrorMessageAsync($"Errore non gestito: {ex.Message}");

#else
			return ShowErrorMessageAsync("Errore non gestito");
#endif
        }

        public virtual Task ShowErrorMessageAsync(string message)
        {
            return DialogService.AlertAsync(message, "JukeBox");
        }

        public virtual Task ShowMessage(string message)
        {
            return DialogService.AlertAsync(message, "JukeBox");
        }
    }
}