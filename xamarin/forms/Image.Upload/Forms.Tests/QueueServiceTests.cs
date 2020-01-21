using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Forms.Services;
using Xunit;

namespace Forms.Tests
{
    public class QueueServiceTests
    {
        [Fact]
        public async Task ShouldDoSomething()
        {
            var queue = new QueueService(2);
            var myList = new List<int>
            {
                2000,
                1000,
                3000,
                1000,
                5000,
                1000
            };

            for (int i = 0; i < myList.Count; i++)
            {
                DateTimeOffset? startTime = null;
                DateTimeOffset? endTime = null;

                var observable =
                    Observable
                        .Return(i)
                        .Do(_ => startTime = startTime ?? DateTimeOffset.Now)
                        .Delay(TimeSpan.FromMilliseconds(myList[i]))
                        .Do(_ => endTime = endTime ?? DateTimeOffset.Now);

                queue
                    .Enqueue(Observable.Create<int>(observer => observable.Subscribe(observer)))
                    .Subscribe(x =>
                        Debug.WriteLine(
                            $"Event: {i}, StartTime: {startTime?.ToString("ss'.'fffffff")}, EndTime: {endTime?.ToString("ss'.'fffffff")}, Delay: {myList[i]}, Value: {x}"));
            }

            Debug.WriteLine("Everything has ended");
        }

        [Fact]
        public async Task ShouldDoSomethingAsync()
        {
            var queue = new QueueService();
            var myList = new List<int>();
            myList.Add(2000);
            myList.Add(1000);
            myList.Add(3000);
            myList.Add(1000);
            myList.Add(5000);
            myList.Add(1000);

            var observables = new List<IObservable<bool>>();
            foreach (var value in myList)
            {
                observables.Add(queue.Enqueue(Observable.FromAsync(async () => await MyWaitMethod(value)).Do(_ => Debug.WriteLine($"Result of task: {_}"))));
            }

            await Observable.Merge(observables).ForEachAsync(_ => { });
            Debug.WriteLine("Everything has ended");
        }

        [Fact]
        public void Should()
        {
            var queue = new QueueService();
            var operation = new Operation();
            var myList = new List<int>
            {
                2000,
                1000,
                3000,
                1000,
                5000,
                1000
            };

            IObservable<OperationResult> request = operation.Execute();
            var result = queue.Enqueue(request);
            result.Subscribe();
        }
        
        [Fact]
        public async Task MasterDoSomethingAsync()
        {
            var queue = new QueueService(2);
            var myList = new List<int>
            {
                2000,
                1000,
                3000,
                1000,
                5000,
                1000
            };

            var tasks = new List<Task>();
            foreach (var value in myList)
            {
                tasks.Add(queue
                    .EnqueueTask(async () => await MyWaitMethod(value))
                    .ContinueWith(_ => { Debug.WriteLine($"Result of task: {_.Result}"); })
                );
            }
            await Task.WhenAll(tasks);
            Debug.WriteLine("Everything has ended");
        }

        public async Task<bool> MyWaitMethod(int waitTime)
        {
            Debug.WriteLine($"MyWaitMethod Start, WaitTime: {waitTime}ms, DateTime: {DateTime.UtcNow}");

            await Task.Delay(waitTime);

            Debug.WriteLine($"MyWaitMethod End, WaitTime: {waitTime}ms, DateTime {DateTime.UtcNow}");
            return true;
        }
    }

    internal class TestOperation : Operation
    {
        
    }
}