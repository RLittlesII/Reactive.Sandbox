using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Animations.Loading
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingButton : ILoadingView
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(LoadingButton));

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(LoadingButton));

        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(LoadingButton));

        public static readonly BindableProperty FrameBackgroundColorProperty = BindableProperty.Create(
            nameof(FrameBackgroundColor),
            typeof(Color),
            typeof(LoadingButton),
            Color.FromHex("#FDE92C"));

        public static readonly BindableProperty FrameBorderColorProperty = BindableProperty.Create(
            nameof(FrameBorderColor),
            typeof(Color),
            typeof(LoadingButton),
            Color.FromHex("#00000000"));

        public static readonly BindableProperty LabelColorProperty = BindableProperty.Create(
            nameof(LabelColor),
            typeof(Color),
            typeof(LoadingButton),
            Color.FromHex("#4a4a4a"));

        public static readonly BindableProperty FrameMarginProperty = BindableProperty.Create(
            nameof(FrameMargin),
            typeof(Thickness),
            typeof(LoadingButton),
            new Thickness(23, 18));

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius),
            typeof(int),
            typeof(LoadingButton),
            2);

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(LoadingButton));

        public static readonly BindableProperty ButtonHeightProperty = BindableProperty.Create(
            nameof(ButtonHeight),
            typeof(int),
            typeof(LoadingButton),
            45);

        public static readonly BindableProperty ElevationProperty =
            BindableProperty.Create(nameof(Elevation), typeof(float), typeof(LoadingButton), 0.0f);

        public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
            nameof(IconSource),
            typeof(string),
            typeof(LoadingButton),
            string.Empty);

        public static readonly BindableProperty HorizontalScaleProperty = BindableProperty.Create(
            nameof(HorizontalScale),
            typeof(LayoutOptions),
            typeof(LoadingButton),
            LayoutOptions.FillAndExpand);

        private static readonly BindableProperty FontScaleProperty = BindableProperty.Create(
            nameof(FontScale),
            typeof(double),
            typeof(LoadingButton),
            1.0);

        private TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();

        public LoadingButton()
        {
            InitializeComponent();

            Tapped =
                Observable
                    .FromEvent<EventHandler, EventArgs>(eventHandler =>
                        {
                            void Handler(object sender, EventArgs args) => eventHandler(args);
                            return Handler;
                        },
                        x => _tapGestureRecognizer.Tapped += x,
                        x => _tapGestureRecognizer.Tapped -= x)
                    .Select(x => Unit.Default);
        }

        public IObservable<Unit> Tapped { get; set; }

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public bool IsLoading
        {
            get => (bool) GetValue(IsLoadingProperty);
            set
            {
                Indicator.IsVisible = value;
                TextStack.IsVisible = !value;
                SetValue(IsLoadingProperty, value);
            }
        }

        public Color FrameBackgroundColor
        {
            get => (Color) GetValue(FrameBackgroundColorProperty);
            set => SetValue(FrameBackgroundColorProperty, value);
        }

        public Color FrameBorderColor
        {
            get => (Color) GetValue(FrameBorderColorProperty);
            set => SetValue(FrameBorderColorProperty, value);
        }

        public Color LabelColor
        {
            get => (Color) GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }

        public Thickness FrameMargin
        {
            get => (Thickness) GetValue(FrameMarginProperty);
            set => SetValue(FrameMarginProperty, value);
        }

        public int CornerRadius
        {
            get => (int) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public new double HeightRequest
        {
            get => TheGrid.HeightRequest;
            set => TheGrid.HeightRequest = value;
        }

        public int ButtonHeight
        {
            get => (int) GetValue(ButtonHeightProperty);
            set => SetValue(ButtonHeightProperty, value);
        }

        public float Elevation
        {
            get => (float) GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public string IconSource
        {
            get => (string) GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }

        public LayoutOptions HorizontalScale
        {
            get => (LayoutOptions) GetValue(HorizontalScaleProperty);
            set => SetValue(HorizontalScaleProperty, value);
        }

        public double FontScale
        {
            get => (double) GetValue(FontScaleProperty);
            set => SetValue(FontScaleProperty, value);
        }

        private void TapGesture_OnTapped(object sender, EventArgs e)
        {
            if (IsEnabled && Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);
        }
    }
}