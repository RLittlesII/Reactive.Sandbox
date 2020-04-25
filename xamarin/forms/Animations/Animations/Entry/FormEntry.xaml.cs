using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Animations.Entry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormEntry
    {
        private const double FocusElevation = 14.0f;

        public static readonly BindableProperty LeftViewProperty = BindableProperty.Create(
            nameof(LeftView),
            typeof(View),
            typeof(FormEntry));

        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
            nameof(Header),
            typeof(string),
            typeof(FormEntry));

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(FormEntry),
            default(string),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (bindable is FormEntry entry
                    && !entry.Entry.IsFocused
                    && !string.IsNullOrEmpty(newValue as string))
                    entry.TheFrame.BackgroundColor = Color.White;
            });

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(FormEntry),
            default(string));

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength),
            typeof(int),
            typeof(FormEntry),
            1000);

        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
            nameof(KeyboardScope),
            typeof(Keyboard),
            typeof(FormEntry),
            Keyboard.Default);

        public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create(
            nameof(ErrorMessage),
            typeof(string),
            typeof(FormEntry),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (bindable is FormEntry instance)
                    instance.ErrorLabel.IsVisible = !string.IsNullOrEmpty(newValue as string);
            });

        public static readonly BindableProperty ErrorSeverityProperty = BindableProperty.Create(
            nameof(ErrorSeverity),
            typeof(Severity),
            typeof(FormEntry),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (bindable is FormEntry instance)
                    instance.ErrorLabel.TextColor = GetColorForSeverity((Severity) newValue);
            });

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(FormEntry),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (bindable is FormEntry instance)
                {
                    instance.PasswordHidden = (bool) newValue;
                    instance.ClearButton.Margin = (bool) newValue
                        ? new Thickness(0, 42, 20, 20)
                        : new Thickness(0, 0, 20, 0);
                }
            });

        public static readonly BindableProperty HidePasswordProperty = BindableProperty.Create(
            nameof(PasswordHidden),
            typeof(bool),
            typeof(FormEntry),
            false);

        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
            nameof(ReturnType),
            typeof(ReturnType),
            typeof(FormEntry),
            ReturnType.Default);

        public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
            nameof(ReturnCommand),
            typeof(ICommand),
            typeof(FormEntry));

        public FormEntry()
        {
            InitializeComponent();
        }

        public View LeftView
        {
            get => (View) GetValue(LeftViewProperty);
            set => SetValue(LeftViewProperty, value);
        }

        public string Header
        {
            get => (string) GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string) GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public int MaxLength
        {
            get => (int) GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public Keyboard KeyboardScope
        {
            get => (Keyboard) GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public string ErrorMessage
        {
            get => (string) GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public Severity ErrorSeverity
        {
            get => (Severity) GetValue(ErrorSeverityProperty);
            set => SetValue(ErrorSeverityProperty, value);
        }

        public bool IsPassword
        {
            get => (bool) GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public bool PasswordHidden
        {
            get => (bool) GetValue(HidePasswordProperty);
            set => SetValue(HidePasswordProperty, value);
        }

        public ReturnType ReturnType
        {
            get => (ReturnType) GetValue(ReturnTypeProperty);
            set => SetValue(ReturnTypeProperty, value);
        }

        public ICommand ReturnCommand
        {
            get => (ICommand) GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }

        public BorderlessEntry Entry => DataValueEntry;

        private void DataValueEntry_OnFocused(object sender, FocusEventArgs e)
        {
            if (Parent is Layout parentLayout)
                parentLayout.RaiseChild(this);

            TheFrame.BackgroundColor = (Color) Resources["FormEntryTheFrameBackgroundColor"];
            TheFrame.Elevation = (float) FocusElevation;

            ScrollToThis().Forget();
        }

        private void DataValueEntry_OnUnfocused(object sender, FocusEventArgs e)
        {
            TheFrame.Elevation = 0;
            TheFrame.BackgroundColor = string.IsNullOrEmpty(DataValueEntry.Text)
                ? (Color) Resources["FormEntryUnfocusedBackgroundColor"]
                : (Color) Resources["FormEntryFocusedBackgroundColor"];
        }

        private async Task ScrollToThis()
        {
            if (this.FindParent<ScrollView>("ScrollViewContainer") is ScrollView scroll)
                //await scroll.ScrollToAsync(0, 250, true);
                //await scroll.ScrollToAsync(this, ScrollToPosition.Start, true);
                Device.StartTimer(TimeSpan.FromSeconds(0.25), () =>
                {
                    scroll.ScrollToAsync(0, 250, true);
                    return false;
                });
        }

        private void FrameTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            DataValueEntry.Focus();
        }

        private void DataValueEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ClearButton.IsVisible = false; // IsEnabled && !string.IsNullOrEmpty(DataValueEntry.Text);
        }

        private static Color GetColorForSeverity(Severity severity)
        {
            switch (severity)
            {
                case Severity.Info:
                    return Color.FromHex("#01daaa");

                case Severity.Warning:
                    return Color.FromHex("#00A4FF");

                default:
                    return Color.FromHex("#d0021b");
            }
        }

        private void Clear_TapGesture_OnTapped(object sender, EventArgs e)
        {
            DataValueEntry.Focus();
            DataValueEntry.Text = string.Empty;
        }

        private void TogglePassword_TapGesture_OnTapped(object sender, EventArgs e)
        {
            DataValueEntry.Focus();
            PasswordHidden = !PasswordHidden;
            ShowHideImage.Source = PasswordHidden ? "eye_cross" : "eye_open";
            ShowHideLabel.Text = PasswordHidden
                ? FormEntryControlResx.ShowPassword
                : FormEntryControlResx.HidePassword;
        }
    }
}