using Xamarin.Forms;

namespace Animations.Behaviors
{
    public class OnlyNumericsAllowedBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            var result = 0;
            if (!int.TryParse(e.NewTextValue, out result))
            {
                string entryText = entry.Text;

                if (!string.IsNullOrEmpty(entryText))
                {
                    entryText = entryText.Remove(entryText.Length - 1);
                    entry.Text = entryText;
                }
            }
        }
    }
}
