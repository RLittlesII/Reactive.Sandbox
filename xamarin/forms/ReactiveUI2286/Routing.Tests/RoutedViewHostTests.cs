using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xunit;

namespace Routing.Tests
{
    public class RoutedViewHostTests
    {
        public class TheNavigateBackCommand
        {
            public TheNavigateBackCommand()
            {
                Locator.CurrentMutable.Register<IScreen>(() => new Screen());
            }

            [Fact]
            public async Task Should_Return_Correct_View_Model()
            {
                // Given
                var viewModel = new TestViewModel();
                var host = new RoutedViewHost { Router = new RoutingState() };

                // When
                await host.Router.Navigate.Execute(viewModel);
                var result = await host.Router.Navigate.Execute(viewModel);

                // Then
                result.Should().Be(viewModel);
            }

            [Fact]
            public async Task Should_Complete_Navigation_Loop()
            {
                // Given
                var testScheduler = new TestScheduler();
                var testViewModel = new TestViewModel();
                var oneTestViewModel = new OneTestViewModel();
                var twoTestViewModel = new TwoTestViewModel();
                var host = new RoutedViewHost { Router = new RoutingState() };

                // When
                await host.Router.Navigate.Execute(testViewModel);
                await host.Router.Navigate.Execute(oneTestViewModel);
                await oneTestViewModel.Confirm.Execute();
                host.Router.NavigationStack.Last().Should().BeOfType<TwoTestViewModel>();
                var result = await host.Router.Navigate.Execute(oneTestViewModel);

                // Then
                result.Should().Be(oneTestViewModel);
            }

            [Fact]
            public async Task Should_Throw()
            {
                // Given
                var host = new RoutedViewHost { Router = new RoutingState() };

                host
                    .Router
                    .NavigateBack
                    .ThrownExceptions
                    .Subscribe(error =>
                    {
                        // Then
                        error.Should().BeOfType<ArgumentOutOfRangeException>();
                    });

                // When
                var result = Record.Exception(() => host.Router.NavigateBack.Execute().Subscribe());

                result.Should().BeOfType<ArgumentOutOfRangeException>();
            }
        }
    }
}