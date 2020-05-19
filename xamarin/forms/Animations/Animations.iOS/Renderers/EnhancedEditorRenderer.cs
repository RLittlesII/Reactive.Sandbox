using Animations;
using Animations.Controls;
using Animations.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EnhancedEditor), typeof(EnhancedEditorRenderer))]

namespace Animations.iOS.Renderers
{
    public class EnhancedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.CornerRadius = 4;
                Control.Layer.BorderColor = Color.FromHex("e0e0e0").ToCGColor();
                Control.Layer.BorderWidth = 1;
            }
        }
    }
}