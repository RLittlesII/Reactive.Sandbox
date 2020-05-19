using Xamarin.Forms;

namespace Animations.Effects
{
    public class ViewShadowEffect : RoutingEffect
    {
        public float Radius { get; set; }

        public Color Color { get; set; }

        public float DistanceX { get; set; }

        public float DistanceY { get; set; }

        public ViewShadowEffect() : base("Quooda.DropShadowEffect")
        {
        }
    }
}
