using System;
using Xamarin.Forms;

namespace Animations.Extensions
{
    public static class FormsViewExtensions
    {
        public static void Shake(
            this VisualElement view,
            bool vertical = false,
            double distance = 15,
            uint steps = 3,
            Action<VisualElement> onStart = null,
            Action<VisualElement> onComplete = null)
        {
            ViewExtensions.CancelAnimations(view);

            Device.BeginInvokeOnMainThread(async () =>
            {
                onStart?.Invoke(view);

                try
                {
                    var stepDistance = distance / steps;
                    var d = distance;
                    do
                    {
                        if (vertical)
                        {
                            await view.TranslateTo(0, -d, 15);
                            await view.TranslateTo(0, d, 15);
                        }
                        else
                        {
                            await view.TranslateTo(-d, 0, 15);
                            await view.TranslateTo(d, 0, 15);
                        }

                        d -= stepDistance;
                    }
                    while (d > -stepDistance);

                    onComplete?.Invoke(view);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public static void BounceAndFadeIn(
            this VisualElement view,
            bool vertical = true,
            double distance = 10,
            uint steps = 3,
            Action<VisualElement> onStart = null,
            Action<VisualElement> onComplete = null)
        {
            ViewExtensions.CancelAnimations(view);

            Device.BeginInvokeOnMainThread(async () =>
            {
                onStart?.Invoke(view);

                try
                {
                    await view.FadeTo(1, 0);

                    await view.FadeTo(0, 1000, Easing.CubicIn);

                    onComplete?.Invoke(view);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }


        public static void ClickEffect(this VisualElement view)
        {
            ViewExtensions.CancelAnimations(view);

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await view.ScaleTo(0.97, 10, Easing.CubicIn);
                    await view.ScaleTo(1, 125, Easing.CubicOut);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public static T FindParent<T>(this VisualElement This, string name = null) where T : class
        {
            if (This == null)
                return null;

            var parent = This.Parent;

            while (parent != null)
            {
                if (name == null && parent is T targetParent)
                    return targetParent;

                if (name != null)
                {
                    try
                    {
                        var element = parent.FindByName<T>(name);

                        if (element != null)
                            return element;
                    }
                    catch (InvalidOperationException)
                    {
                        // raised when "this element is not in a namescope". It is safe to ignore it
                    }
                }

                parent = parent.Parent;
            }

            return null;
        }
    }
}
