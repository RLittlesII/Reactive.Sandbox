using System;
using Animations.Extensions;
using Xamarin.Forms;

namespace Animations.Behaviors
{
    public class ClickEffectBehavior : Behavior<View>
    {
        private TapGestureRecognizer tapGestureRecognizer;

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            if (bindable is View view)
            {
                SetUpGestureRecognizer(view);
            }
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            if (bindable is View view)
            {
                tapGestureRecognizer.Tapped -= TapGestureRecognizer_Tapped;
                bindable.GestureRecognizers.Remove(tapGestureRecognizer);
                tapGestureRecognizer = null;
            }
        }

        private void SetUpGestureRecognizer(View view)
        {
            tapGestureRecognizer = new TapGestureRecognizer();
            view.GestureRecognizers.Add(tapGestureRecognizer);
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is View view)
                view.ClickEffect();
        }
    }
}
