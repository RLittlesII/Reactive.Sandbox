using System;
using System.Collections;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using Xamarin.Forms;

namespace Dialog.Actions
{
    public class Actions : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(ActionSheetDetail),
                default(IEnumerable),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnItemSourcePropertyChanged);

        private static void OnItemSourcePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var action = bindable as Actions;
            IEnumerable oldList = oldvalue as IEnumerable;
            IEnumerable newList = newvalue as IEnumerable;
            if (action != null)
            {
                action.ItemsSource = Enumerable.Empty<object>();

                if (newList == null)
                {
                    return;
                }

                foreach (var detail in newList)
                {
                    if (detail is ActionSheetDetail actionSheetDetail)
                    {
                        ((IList) action.ItemsSource).Add(actionSheetDetail);
                    }
                }

                ItemTapped = Observable.Create<TappedEventArgs>(observer =>
                    action
                        .ItemsSource
                        .Cast<ActionSheetDetail>()
                        .Select(x => x.Tapped)
                        .Merge()
                        .Subscribe(observer));
            }
        }

        public static IObservable<TappedEventArgs> ItemTapped { get; set; }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                "ItemTemplate",
                typeof(DataTemplate),
                typeof(ActionSheetDetail),
                default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }
    }
}