using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Animations.Extensions
{
    // Required to get the assembly name for resoures
    public class IncludeInBuild
    {
    }
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
		private static readonly Lazy<ResourceManager> ResourcesMgr = 
            new Lazy<ResourceManager>(() => 
               new ResourceManager(
                   "TransactionResourceId", 
                   typeof(IncludeInBuild).GetTypeInfo().Assembly));


        private readonly CultureInfo _cultureInfo;

        public string Text { get; set; }

        public TranslateExtension()
        {
            if (Device.iOS == Device.RuntimePlatform || Device.Android == Device.RuntimePlatform)
            {
                //_cultureInfo = CrossMultilingual.Current.DeviceCultureInfo;
            }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return string.Empty;
            }

            var translation = ResourcesMgr.Value.GetString(Text, _cultureInfo);

            if (translation == null)
            {
                //Crashes.TrackError(new ArgumentException(
                //    string.Format($"Key '{Text}', was not found in resources '{ApplicationKeys.TranslationResourceId}' for culture '{_cultureInfo.Name}'")));

                translation = Text; // returns the key, which gets displayed to the user
            }

            return translation;
        }

        public string Translate(string key)
        {
            return ResourcesMgr.Value.GetString(key, _cultureInfo);
        }
    }
}
