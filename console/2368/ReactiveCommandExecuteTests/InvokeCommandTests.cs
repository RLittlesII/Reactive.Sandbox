using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.Testing;
using Xunit;

namespace ReactiveCommandExecuteTests
{
    public class InvokeCommandTests
    {
[Fact]
public void InvokeCommandTest() => new TestScheduler().WithAsync(async scheduler =>
{
    var fixture = ReactiveCommand.CreateFromTask(() => TaskAsync(scheduler));

    var didFire = false;
    fixture.Subscribe(_ => didFire = true);

    Observable.Return(Unit.Default).InvokeCommand(fixture);

    Assert.True(didFire);
});

[Fact]
public void InvokeCommandTestExecute() => new TestScheduler().With(scheduler =>
{
    var fixture = ReactiveCommand.CreateFromTask(() => TaskAsync(scheduler));
    var didFire = false;

    fixture.Execute(Unit.Default).Subscribe(_ =>
    {
        Assert.Equal(1, _);
    });
});

private static int count = 0;

private async Task<int> TaskAsync(IScheduler scheduler)
{
    await Observable.Return(Unit.Default).Delay(TimeSpan.FromSeconds(3), scheduler);
    return 1;
}
    }
}