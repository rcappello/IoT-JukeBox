using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using JukeBox.Mobile.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellTransparentRenderer))]
namespace JukeBox.Mobile.iOS
{
    public class ViewCellTransparentRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                // Disable native cell selection color style - set as *Transparent*
                cell.SelectionStyle = UITableViewCellSelectionStyle.Gray;
            }
            return cell;
        }
    }
}