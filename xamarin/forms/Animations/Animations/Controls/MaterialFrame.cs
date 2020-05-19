using Xamarin.Forms;

namespace Animations.Controls
{
    public class MaterialFrame : Frame
    {
        public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
            nameof(Elevation),
            typeof(float),
            typeof(MaterialFrame),
            4.0f);

        public float Elevation
        {
            get => (float) GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }
    }
}