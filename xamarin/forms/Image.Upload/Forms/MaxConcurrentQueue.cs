using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Forms.Services;
using ReactiveUI;

namespace Forms
{
    public class MaxConcurrentQueue<T>
    {
        private ConcurrentQueue<IOperation> _operationQueue = new ConcurrentQueue<IOperation>();
        private List<IOperation> _maxList = new List<IOperation>();
        private ScheduledSubject<IOperation> _subject;
        private Subject<IObservable<Unit>> _doWork = new Subject<IObservable<Unit>>();

        public MaxConcurrentQueue()
        {
            _doWork
                .Merge(3)
                .Subscribe();
        }

        public void Enqueue(IOperation thing)
        {
            _operationQueue.Enqueue(thing);

            _subject.OnNext(thing);
            // OnNext will call Dequeue somewhere too?


        }

        public IObservable<T> Enqueue<T>(IObservable<T> request)
        {
            var obs2 = Observable.Create<T>(observer => {
                _doWork.OnNext(request
                    .Do(observer)
                    .Materialize()
                    .Where(z => z.Kind == NotificationKind.OnCompleted)
                    .Select(z => Unit.Default)
                );
                return Disposable.Empty;
            });

            return obs2.Publish().RefCount();
        }

        public Task<T> EnqueueTask<T>(Func<Task<T>> request)  {
            return Enqueue(Observable.FromAsync(request)).ToTask();
        }

        public IObservable<T> Dequeued { get; }
    }

    public class OperationResult
    {
        public bool Success { get; set; }

        public IOperation Operation { get; set; }

        public OperationFailureException Exception { get; set; }
    }

    public class OperationFailureException
    {
        public int Stage { get; set; }  // TODO: Make this better
    }

    public class Stalls : ReactiveObject
    {
        private bool _first;
        private bool _second;
        private bool _third;

        public bool First
        {
            get => _first;
            set => this.RaiseAndSetIfChanged(ref _first, value);
        }

        public bool Second
        {
            get => _second;
            set => this.RaiseAndSetIfChanged(ref _second, value);
        }

        public bool Third
        {
            get => _third;
            set => this.RaiseAndSetIfChanged(ref _third, value);
        }
    }

    /*
    var doWork = new Subject<IObservable<DateTimeOffset>>();
var i = 0;

var dw = doWork.Publish().RefCount();
var waiter = dw
	.Merge(3)
	.Subscribe();

var rand = new Random();
for (var j = 0; j <= 100; j++)
{
	{
		var a = j;
		var delay = rand.Next(1000, 5000);
		DateTimeOffset? startTime = null;
		DateTimeOffset? endTime = null;
		Enqueue(
			Observable.Create<int>(o =>
			{
				startTime = DateTimeOffset.Now;
				return Observable.Range(1, rand.Next(1, 3))
					.Delay(TimeSpan.FromMilliseconds(delay))
					.Do(_ => { 
						endTime ??= DateTimeOffset.Now;
					})
					.Subscribe(o);
			})
		)
			.Subscribe(x => $"Event: {a}, StartTime: {startTime?.ToString("ss'.'fffffff")}, EndTime: {endTime?.ToString("ss'.'fffffff")}, Delay: {delay}, Value: {x}".Dump());
	}
}


IObservable<T> Enqueue<T>(IObservable<T> request)
{
	var obs2 = Observable.Create<T>(observer =>
	{
		doWork.OnNext(request
			.Do(observer)
			.Materialize()
			.Where(z => z.Kind == NotificationKind.OnCompleted)
			.Select(z => DateTimeOffset.Now)
		);
		return Disposable.Empty;
	});

	return obs2.Publish().RefCount();
}

Task<T> EnqueueTask<T>(Func<Task<T>> request)
{
	return Enqueue(Observable.FromAsync(request)).ToTask();
}
Task<T> EnqueueTaskCt<T>(Func<CancellationToken, Task<T>> request)
{
	return Enqueue(Observable.FromAsync(request)).ToTask();
}

await dw.Do(_ => { }, e => e.Dump()).ForEachAsync(x => { });





code 2:
var doWork = new Subject<IObservable<DateTimeOffset>>();
var i = 0;

var buffer = new List<IObservable<DateTimeOffset>>();
var pauseCommand = new BehaviorSubject<bool>(false);


var dw = doWork.Publish().RefCount();
var waiter = dw
	.CombineLatest(pauseCommand, (observable, pause) => (observable, pause))
	.SelectMany(_ =>
	{
		var (observable, pause) = _;
		if (pause)
		{
			buffer.Add(observable);
			return Observable.Empty<IObservable<DateTimeOffset>>();
		}
		if (buffer.Any())
		{
			var r = Observable.Concat(buffer.ToArray().ToObservable(), Observable.Return(observable));
			buffer.Clear();
			return r;
		}
		return Observable.Return(observable);
	})
	//.Do(x => "after pause".Dump())
	.Merge(3)
	//.Do(x => "after merge".Dump())
	.Subscribe();

var rand = new Random();
for (var j = 0; j <= 100; j++)
{
	if (j % 5 == 0)
	{
		pauseCommand.OnNext(!pauseCommand.Value);
	}
	{
		var a = j;
		var delay = rand.Next(1000, 5000);
		DateTimeOffset? startTime = null;
		DateTimeOffset? endTime = null;
		Enqueue(
			Observable.Create<int>(o =>
			{
				startTime = DateTimeOffset.Now;
				return Observable.Range(1, rand.Next(1, 3))
					.Delay(TimeSpan.FromMilliseconds(delay))
					.Do(_ =>
					{
						endTime ??= DateTimeOffset.Now;
					})
					.Subscribe(o);
			})
		)
			.Subscribe(x => $"Event: {a}, StartTime: {startTime?.ToString("ss'.'fffffff")}, EndTime: {endTime?.ToString("ss'.'fffffff")}, Delay: {delay}, Value: {x}".Dump());
	}
	if (j > 0 && j % 5 == 0)
	{
		var wait = rand.Next(2000, 10000);
		$"Waiting {wait} (pause: {pauseCommand.Value})".Dump();
		await Task.Delay(wait);
	}
}


IObservable<T> Enqueue<T>(IObservable<T> request)
{
	var obs2 = Observable.Create<T>(observer =>
	{
		doWork.OnNext(request
			.Do(observer)
			.Materialize()
			.Where(z => z.Kind == NotificationKind.OnCompleted)
			.Select(z => DateTimeOffset.Now)
		);
		return Disposable.Empty;
	});

	return obs2.Publish().RefCount();
}

Task<T> EnqueueTask<T>(Func<Task<T>> request)
{
	return Enqueue(Observable.FromAsync(request)).ToTask();
}
Task<T> EnqueueTaskCt<T>(Func<CancellationToken, Task<T>> request)
{
	return Enqueue(Observable.FromAsync(request)).ToTask();
}

await dw.Do(_ => { }, e => e.Dump()).ForEachAsync(x => { });




    */
}