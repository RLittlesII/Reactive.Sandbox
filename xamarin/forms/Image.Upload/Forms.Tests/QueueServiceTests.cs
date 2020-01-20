using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Forms.Services;
using Xunit;

namespace Forms.Tests
{
    public class QueueServiceTests
    {
        Task<T> Function<T>()
        {
            Task.Delay(TimeSpan.MaxValue);
            return Task.FromResult(default(T));
        }

        [Fact]
        public void Should()
        {
            // Given
            var queue = new QueueService();

            // When
            var result = queue.EnqueueTask<int>(Function<int>);
        }

        [Fact]
        public async void ShouldDoSomething()
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
        public async void ShouldDoSomethingAsync()
        {
            var queue = new QueueService();
            var myList = new List<int>();
            myList.Add(2000);
            myList.Add(1000);
            myList.Add(3000);
            myList.Add(1000);
            myList.Add(5000);
            myList.Add(1000);
            foreach (var value in myList)
            {
                var observable = Observable.Create<bool>(_ =>
                {
                    return Observable.FromAsync(async () => await MyWaitMethod(value)).Subscribe();
                });
                queue
                    .Enqueue(observable)
                    .Subscribe(_ => Debug.WriteLine($"Result of task: {_}"));
            }
            Debug.WriteLine("Everything has ended");
        }
        
        [Fact]
        public async void MasterDoSomethingAsync()
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

            foreach (var value in myList)
            {
                var result = queue.EnqueueTask(async () => await MyWaitMethod(value));
                Debug.WriteLine($"Result of task: {result.Result}");
            }
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
}