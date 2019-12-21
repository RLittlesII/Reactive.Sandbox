using System;
using FluentAssertions;
using Forms.Services;
using Forms.Types;
using Xunit;

namespace Forms.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Return_Queued()
        {
            // Given
            var sut = new UploadService();
            UploadState state = UploadState.Dequeued;
            sut.Queued.Subscribe(x => state = x.State);
            
            // When
            sut.Queue(new UploadPayload { Id = "10" });

            // Then
            state.Should().Be(UploadState.Queued);
        }
    }
}
