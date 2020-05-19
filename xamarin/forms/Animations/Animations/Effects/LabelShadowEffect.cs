using Xamarin.Forms;

namespace Animations.Effects
{
    public class LabelShadowEffect : RoutingEffect
    {
        public float Radius { get; set; }

        public Color Color { get; set; }

        public float DistanceX { get; set; }

        public float DistanceY { get; set; }

        public LabelShadowEffect()
            : base($"{nameof(Animations)}.{nameof(LabelShadowEffect)}") { }
    }
}
