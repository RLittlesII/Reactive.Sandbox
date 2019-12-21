using Forms.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;

namespace Forms.Services
{
    public interface IUploadService
    {
        // When the user clicks the upload
        // When the internet is available
        // When the item is added to the queue
        // When the upload starts
        // When the upload finishes
        // Whether the upload is successful
        // When something is removed from the queue
        // When the user turns the service on
        // When the user turns the service off

        /// <summary>
        /// An observable sequence that notifies of changes to the internal queue.
        /// </summary>
        IObservable<UploadEventArgs> Queued { get; }

        UploadPayload CurrentUpload { get; }

        /// <summary>
        /// Adds a payload to the queue.
        /// </summary>
        /// <param name="payload"></param>
        void Queue(UploadPayload payload);

        /// <summary>
        /// Adds a payload to the queue.
        /// </summary>
        /// <param name="payload"></param>
        void Queue(IEnumerable<UploadPayload> payload);

        /// <summary>
        /// Pauses the queue.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> Resume();
        
        /// <summary>
        /// Pauses the queue.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> Pause();
    }
}