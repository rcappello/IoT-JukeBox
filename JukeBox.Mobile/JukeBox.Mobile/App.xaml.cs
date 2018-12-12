using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace JukeBox.Mobile
{
    public partial class App : Application
    {
        public static bool IsPausing { get; set; }
        
        public App()
        {
            InitializeComponent();

            var mainpage = new Views.MainPage();
            var navigationPage = new NavigationPage(mainpage);
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
