using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Timers;

namespace Stopwatch
{
    class Program
    {
        private static ILogger _logger = Serilog.Core.Logger.None;

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            _logger = Log.Logger;
            _logger.Debug("Hello World!");

            var timer = new DecrementTimer(TaskPoolScheduler.Default);

            var task = timer
                .Timer(TimeSpan.FromSeconds(3), false)
                .SubscribeOn(Scheduler.Immediate)
                .TakeWhile(_ => _ >= TimeSpan.Zero)
                .ObserveOn(Scheduler.Default)
                .ForEachAsync(_ => _logger.Information(_.ToString()));

            timer
                .Start()
                .Subscribe(_=> _logger.Warning("Started"));

            timer
                .Pause()
                .Subscribe(_=> _logger.Warning("Paused"));

            Observable.Return(Unit.Default)
                .SubscribeOn(TaskPoolScheduler.Default)
                .Delay(TimeSpan.FromSeconds(1))
                .ObserveOn(Scheduler.Default)
                .Subscribe();

            timer
                .Resume()
                .Subscribe(_=> _logger.Warning("Resumed"));

            await task;
            Log.CloseAndFlush();
        }
    }
}