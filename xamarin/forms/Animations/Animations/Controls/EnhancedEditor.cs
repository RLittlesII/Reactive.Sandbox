using Xamarin.Forms;

namespace Animations.Controls
{
    public class EnhancedEditor : Editor
    {
        public EnhancedEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~EnhancedEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}