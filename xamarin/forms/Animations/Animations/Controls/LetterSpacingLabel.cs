using Xamarin.Forms;

namespace Animations.Controls
{
    public class LetterSpacingLabel : Label
    {
        public static readonly BindableProperty LetterSpacingProperty = BindableProperty.Create(
            nameof(LetterSpacing),
            typeof(float),
            typeof(LetterSpacingLabel));

        public float LetterSpacing
        {
            get => (float) GetValue(LetterSpacingProperty);
            set => SetValue(LetterSpacingProperty, value);
        }
    }
}