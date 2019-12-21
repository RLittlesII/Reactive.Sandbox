using System.Threading.Tasks;
using Punchclock;
using Xunit;

namespace Forms.Tests
{
    public class OperationQueueTests
    {
        [Fact]
        public void Should()
        {
            // Given
            var queue = new OperationQueue(2);

            // When
            var task = Task.FromResult(default(int));

            var first = queue.Enqueue(1, () => task);
            var second= queue.Enqueue(1, () => task);
            var thirds =queue.Enqueue(1, () => task);
            var fourth =queue.Enqueue(1, () => task);

            Task.WaitAll(first, second, thirds, fourth);

            // Then
        }
        
    }
}