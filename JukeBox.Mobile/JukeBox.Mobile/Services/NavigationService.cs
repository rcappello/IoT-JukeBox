using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using JukeBox.Mobile.Common;
using JukeBox.Mobile.Controls;

namespace JukeBox.Mobile.Services
{
	public class NavigationService : INavigationService
	{
		private Dictionary<string, Type> pages { get; } = new Dictionary<string, Type>();
		NavigationPage navigationPage;

		public string CurrentPageKey { get; private set; }

		public Page MainPage
		{
			get
			{
				var page = Application.Current.MainPage;
				if (page is MasterDetailPage)
					page = (page as MasterDetailPage).Detail;

				return page;
			}
			set{
				var page = Application.Current.MainPage;
				if (page is MasterDetailPage)
					(page as MasterDetailPage).Detail = value;
				else
					Application.Current.MainPage = value;
			}
		}

		public void Configure(string key, Type pageType) => pages.Add(key, pageType);

		#region INavigationService implementation

		public void GoBack()
		{
			if (MainPage.Navigation.ModalStack.Count > 0)
				MainPage.Navigation.PopModalAsync();
			else
				MainPage.Navigation.PopAsync();
		}

		public void NavigateTo(string pageKey) => NavigateTo(pageKey, null);

		public void NavigateTo(string pageKey, object parameter) => NavigateTo(pageKey, parameter, HistoryBehavior.Default, PreAppearingBehavior.Default);

		public void NavigateTo(string pageKey, HistoryBehavior historyBehavior) => NavigateTo(pageKey, null, historyBehavior, PreAppearingBehavior.Default);

		public void NavigateTo(string pageKey, HistoryBehavior historyBehavior, PreAppearingBehavior preAppearingBehavior) => NavigateTo(pageKey, null, historyBehavior, preAppearingBehavior);

		public void NavigateTo(string pageKey, object parameter, HistoryBehavior historyBehavior, PreAppearingBehavior preAppearingBehavior = PreAppearingBehavior.Default)
		{
			Type pageType;
			if (pages.TryGetValue(pageKey, out pageType))
			{
				var displayPage = (Page)Activator.CreateInstance(pageType, args: parameter);
				CurrentPageKey = pageKey;

				((IContentPageBase)displayPage)?.OnPreAppearing(preAppearingBehavior == PreAppearingBehavior.ClearData);

				if (historyBehavior == HistoryBehavior.ClearHistory)
				{
					displayPage.SetNavigationArgs(parameter);

                    ((NavigationPage)MainPage).CurrentPage.Navigation.InsertPageBefore(displayPage, ((NavigationPage)MainPage).RootPage);
					MainPage.Navigation.PopToRootAsync();
				}
				else
				{
					MainPage.Navigation.PushAsync(displayPage, parameter, animated: true);
				}
			}
			else
			{
				throw new ArgumentException(
						  $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
						  nameof(pageKey));
			}
		}

		public void NavigateModalTo(string pageKey) => NavigateModalTo(pageKey, null);

		public void NavigateModalTo(string pageKey, object parameter, bool useNavigation = false, PreAppearingBehavior preAppearingBehavior = PreAppearingBehavior.Default)
		{
			Type pageType;
			if (pages.TryGetValue(pageKey, out pageType))
			{
				var displayPage = (Page)Activator.CreateInstance(pageType, args: parameter);

                ((IContentPageBase)displayPage)?.OnPreAppearing(preAppearingBehavior == PreAppearingBehavior.ClearData);

                if (useNavigation)
				{
					var navPage = new AppNavigationPage(displayPage);
					displayPage.SetNavigationArgs(parameter);
					MainPage.Navigation.PushModalAsync(navPage, parameter, animated: true);
				}
				else
				{
					MainPage.Navigation.PushModalAsync(displayPage, parameter, animated: true);
				}
			}
			else
			{
				throw new ArgumentException(
						  $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
						  nameof(pageKey));
			}            
		}

		public void CloseModal()
		{
			MainPage.Navigation.PopModalAsync();
		}

		public void GoHome()
		{
			MainPage.Navigation.PopToRootAsync();
		}

		#endregion
	}
}
