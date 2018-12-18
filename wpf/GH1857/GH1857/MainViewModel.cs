using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace GH1857
{
    public class MainViewModel : ReactiveObject, IDisposable
    {
        private Subject<(string title, string text)> _onMessage = new Subject<(string title, string text)>();
        private CompositeDisposable disposable = new CompositeDisposable();

        public ReactiveCommand<Unit, string> CreateProjectCommand { get; set; }

        public IObservable<(string title, string text)> OnMessage => _onMessage.AsObservable();

        public MainViewModel()
        {
            CreateProjectCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await Task.Delay(1000);
                return "SomeValue";
            });

            CreateProjectCommand
                .Where(r => r != null)
                .Select(r =>
                {
                    //breakpoint here
                    return ("Успешно", $"Проект '{r}' успешно создан.");
                })
                .Subscribe(_onMessage)
                .DisposeWith(disposable);
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref disposable, null)?.Dispose();
        }
    }
}