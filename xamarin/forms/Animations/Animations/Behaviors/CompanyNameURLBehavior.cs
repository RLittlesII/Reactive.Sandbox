using Xamarin.Forms;

namespace Animations.Behaviors
{
    public class CompanyNameBehavior : Behavior<Entry>
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

            if (!string.IsNullOrEmpty(entry?.Text))
            {
                string text = entry?.Text;

                text = text.Replace(" ", "")
                           .Replace(".", "")
                           .Replace(";", "")
                           .Replace(":", "")
                           .Replace("/", "")
                           .Replace("@", "")
                           .Replace("#", "")
                           .Replace("?", "")
                           .Replace("*", "")
                           .Replace("$", "");

                entry.Text = text;
            }
        }
    }
}