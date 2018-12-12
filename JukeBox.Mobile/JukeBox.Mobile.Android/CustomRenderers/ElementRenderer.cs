using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using static JukeBox.Mobile.Droid.ElementRenderer;

[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.Forms.ViewCell), typeof(ViewCellTransparentRenderer))]
namespace JukeBox.Mobile.Droid
{
    public class ElementRenderer : VisualElementRenderer<Xamarin.Forms.View>
    {
        public ElementRenderer(Context context): base (context)
        {

        }
        public class ViewCellTransparentRenderer : ViewCellRenderer
        {
            protected override View GetCellCore(Xamarin.Forms.Cell item, View convertView, ViewGroup parent, Context context)
            {
                var cell = base.GetCellCore(item, convertView, parent, context);

                if (parent is Android.Widget.ListView listView)
                {
                    // Disable native cell selection color style - set as *Transparent*
                    listView.SetSelector(Android.Resource.Color.Transparent);
                    listView.CacheColorHint = Android.Graphics.Color.Transparent;
                }

                return cell;
            }
        }
    }
}