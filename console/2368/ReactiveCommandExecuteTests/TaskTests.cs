using System;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.Testing;
using Xunit;

namespace ReactiveCommandExecuteTests
{
    public class TaskTests
    {
        // https://stackoverflow.com/questions/53728846/async-method-deadlocks-with-testscheduler-in-reactiveui
        [Fact]
        public void ToTaskSchedulerTest() => new TestScheduler().WithAsync(async scheduler =>
        {
            var fixture = ReactiveCommand.CreateFromTask(TaskAsync);
            var task = fixture.Execute().ToTask();
            scheduler.Start();

            var result = await task;

            Assert.Equal(result, 1);
        });

        private async Task<int> TaskAsync()
        {
            await Task.Delay(1000);
            return 1;
        }
    }
}
