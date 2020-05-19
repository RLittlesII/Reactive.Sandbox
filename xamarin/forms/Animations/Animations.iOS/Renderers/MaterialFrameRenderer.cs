using System.ComponentModel;
using System.Drawing;
using Animations;
using Animations.Controls;
using Animations.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MaterialFrame), typeof(MaterialFrameRenderer))]
namespace Animations.iOS.Renderers
{
    public class MaterialFrameRenderer : FrameRenderer
    {
        public static void Initialize()
        {
            // empty, but used for beating the linker
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                UpdateShadow();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == MaterialFrame.ElevationProperty.PropertyName)
            {
                UpdateShadow();
            }
        }

        private void UpdateShadow()
        {
            var materialFrame = (MaterialFrame)Element;

            materialFrame.HasShadow = materialFrame.Elevation > 0;
            // Update shadow to match better material design standards of elevation
            Layer.ShadowRadius = materialFrame.Elevation > 0 ? materialFrame.Elevation / 2.6f : 0.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new SizeF();
            Layer.ShadowOpacity = materialFrame.Elevation > 0 ? 0.35f : 0.0f;
        }
    }
}
