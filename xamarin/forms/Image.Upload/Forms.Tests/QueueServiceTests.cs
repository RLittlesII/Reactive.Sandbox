using System;
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
    }
}