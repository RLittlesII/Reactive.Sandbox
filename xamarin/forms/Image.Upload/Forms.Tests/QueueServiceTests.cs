using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Forms.Services;
using ReactiveUI;
using Xunit;

namespace Forms.Tests
{
    public class QueueServiceTests
    {
        [Fact]
        public async Task ShouldDoSomething()
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

            await Observable.Merge(observables).ForEachAsync(_ => { /* Process Result Here */ });
            Debug.WriteLine("Everything has ended");
        }

        [Fact]
        public async Task ShouldDoSomethingAsync()
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
                    .Enqueue(async () => await MyWaitMethod(value))
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
        public override IObservable<OperationResult> Execute() =>
            Observable.Return(default(OperationResult));
    }
}