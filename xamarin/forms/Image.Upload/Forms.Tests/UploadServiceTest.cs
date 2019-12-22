using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Forms.Services;
using Forms.Types;
using Xunit;

namespace Forms.Tests
{
    public sealed class UploadServiceTest
    {
        public class TheQueuedProperty
        {
            [Fact]
            public void Should_Notify_State_Changed()
            {
                // Given
                var sut = new UploadService();
                List<UploadState> state = Array.Empty<UploadState>().ToList();
                sut.Queued.Subscribe(x => state.Add(x.State));

                // When
                sut.Queue(new UploadPayload { Form = new Form { Id = "10" } });

                // Then
                state[0].Should().Be(UploadState.Queued);
                state[1].Should().Be(UploadState.UploadStarted);
                state[2].Should().Be(UploadState.UploadCompleted);
                state[3].Should().Be(UploadState.Dequeued);
            }

            [Fact]
            public void Should_Notify_Error()
            {
                // Given
                var sut = new UploadService();
                UploadState state = UploadState.Queued;
                sut.Queued.Subscribe(x => state = x.State);

                // When
                sut.Queue(new UploadPayload { Form = new Form { Id = "10" } });

                // Then
                state.Should().Be(UploadState.Errored);
            }
        }
    }
}
