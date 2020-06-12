using AppKit;
using Forms;
using Foundation;
using Xamarin.Forms.Platform.MacOS;

namespace macOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow window;
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Borderless | NSWindowStyle.Titled | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable;

            var rect = new CoreGraphics.CGRect(200, 1000, 350, 575);
            window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            window.Title = "Xamarin on Mac!"; // choose your own Title here
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow => window;

        public override void DidFinishLaunching(NSNotification notification)
        {
            Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }
    }
}