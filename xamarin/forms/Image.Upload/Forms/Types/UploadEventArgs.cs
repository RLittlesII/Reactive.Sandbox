using System;
using Forms.Types;

namespace Forms.Services
{
    public class UploadEventArgs
    {
        public DateTimeOffset? UploadedTime { get; set; }

        public UploadState State { get; set; }

        public string Id { get; set; }
    }
}