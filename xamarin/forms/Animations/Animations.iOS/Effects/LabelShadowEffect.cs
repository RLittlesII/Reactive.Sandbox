using System.Linq;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsLabelShadowEffect = Animations.Effects.LabelShadowEffect;

[assembly:ExportEffect (typeof(FormsLabelShadowEffect), nameof(FormsLabelShadowEffect))]

namespace Animations.iOS.Effects
{
    public class LabelShadowEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var effect = (FormsLabelShadowEffect)Element?.Effects?.FirstOrDefault(e => e is FormsLabelShadowEffect);
            if (effect == null || Control == null)
                return;

            Control.Layer.CornerRadius = effect.Radius;
            Control.Layer.ShadowColor = effect.Color.ToCGColor();
            Control.Layer.ShadowOffset = new CGSize(effect.DistanceX, effect.DistanceY);
            Control.Layer.ShadowOpacity = 1.0f;
        }

        protected override void OnDetached()
        {
        }
    }
}
