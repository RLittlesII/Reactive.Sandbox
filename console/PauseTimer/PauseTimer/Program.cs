using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace PauseTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var xs = Observable.Generate(
                0,
                x => x < 100,
                x => x + 1,
                x => x,
                x => TimeSpan.FromSeconds(0.1));

            var bs = new Subject<bool>();

            var pxs = xs.Pausable(bs);

            pxs.Subscribe(x => { Console.WriteLine(x.ToString()); });

            Thread.Sleep(500);
            bs.OnNext(true);
            Thread.Sleep(5000);
            bs.OnNext(false);
            Thread.Sleep(500);
            bs.OnNext(true);
            Thread.Sleep(5000);
            bs.OnNext(false);
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pauser"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>https://stackoverflow.com/questions/7620182/pause-and-resume-subscription-on-cold-iobservable</remarks>
        // Here's a reasonably simple Rx way to do what you want. I've created an extension method called Pausable that takes a source observable and a second observable of boolean that pauses or resumes the observable.
        public static IObservable<T> Pausable<T>(this IObservable<T> source, IObservable<bool> pauser, bool startPaused = false)
        {
            return Observable.Create<T>(o =>
            {
                var subscriptions = new SerialDisposable();
                var replaySubjects = new SerialDisposable();

                var subscription = source.Publish(stream =>
                {
                    Func<ReplaySubject<T>> replaySubjectFactory = () =>
                    {
                        var rs = new ReplaySubject<T>();

                        replaySubjects.Disposable = rs;
                        subscriptions.Disposable = stream.Subscribe(rs);

                        return rs;
                    };

                    var replaySubject = replaySubjectFactory();

                    Func<bool, IObservable<T>> switcher = isPaused =>
                    {
                        if (isPaused)
                        {
                            replaySubject = replaySubjectFactory();

                            return Observable.Empty<T>();
                        }
                        else
                        {
                            return replaySubject.Concat(stream);
                        }
                    };

                    return pauser
                        .StartWith(startPaused)
                        .DistinctUntilChanged()
                        .Select(switcher)
                        .Switch();
                }).Subscribe(o);

                return new CompositeDisposable(subscription, subscriptions);
            });
        }
    }
}