using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using JukeBox.Mobile.Droid.CustomRenderers;
using Android.Widget;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(AwesomeRenderer))]
namespace JukeBox.Mobile.Droid.CustomRenderers
{
    public class AwesomeRenderer : ButtonRenderer
    {
        private Context context;

        public AwesomeRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var button = (Android.Widget.Button)Control;
            var text = button.Text;
            if (text.Length == 0) return;

            if (text.Length > 1 || text[0] < 0xf000)
            {
                return;
            }

            var font = Typeface.CreateFromAsset(context.ApplicationContext.Assets, "fontawesome.ttf");
            button.Typeface = font;
        }
    }
}
