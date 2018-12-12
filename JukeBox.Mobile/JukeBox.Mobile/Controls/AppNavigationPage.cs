using Xamarin.Forms;

namespace JukeBox.Mobile.Controls
{
    public class AppNavigationPage : NavigationPage
    {
        public AppNavigationPage(Page root) : base(root)
        {
            Init();
        }

        public AppNavigationPage()
        {
            Init();
        }

        void Init()
        {
            BarBackgroundColor = Color.FromHex("#0000FF");
            BarTextColor = Color.White;
        }
    }
}
