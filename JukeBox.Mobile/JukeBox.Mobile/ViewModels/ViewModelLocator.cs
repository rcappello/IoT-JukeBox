using Acr.UserDialogs;
using GalaSoft.MvvmLight.Ioc;
using JukeBox.Mobile.Common;
using JukeBox.Mobile.Services;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Mobile.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(Constants.MainPage, typeof(Views.MainPage));

            SimpleIoc.Default.Register<NavigationService>(() => navigationService);
            SimpleIoc.Default.Register<INetworkService, NetworkService>();
            SimpleIoc.Default.Register<IAzureService, AzureService>();
            SimpleIoc.Default.Register<IJukeBoxService, JukeBoxService>();
            SimpleIoc.Default.Register<IUserDialogs>(() => UserDialogs.Instance);

            SimpleIoc.Default.Register<MainViewModel>();

        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}
