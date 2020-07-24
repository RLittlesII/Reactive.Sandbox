using System;
using ReactiveUI;

namespace WhenAny.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainViewModel = new MainViewModel();

            mainViewModel
                .WhenAnyValue(x => x.TimeSync)
                .Subscribe(System.Console.WriteLine);

            System.Console.ReadLine();
        }
    }
}