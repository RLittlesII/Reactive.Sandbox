using System;
using FluentAssertions;
using Forms.Services;
using Xunit;

namespace Forms.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Return_Queued()
        {
            // Given
            var sut = new ImageUploadService();
            UploadState state = UploadState.Dequeued;
            sut.Queued.Subscribe(x => state = x.State);
            
            // When
            sut.Queue(new MyTestPayload { Id = 10 });

            // Then
            state.Should().Be(UploadState.Queued);
        }
    }
}
