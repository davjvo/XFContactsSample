using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFContactsSample.iOS.Renders;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace XFContactsSample.iOS.Renders
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 10;
                Control.TextColor = UIColor.White;
            }
        }
    }
}