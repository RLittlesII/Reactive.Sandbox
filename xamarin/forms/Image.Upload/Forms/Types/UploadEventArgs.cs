using System;

namespace Forms.Services
{
    public class UploadEventArgs
    {
        public DateTimeOffset? UploadedTime { get; set; }

        public UploadState State { get; set; }

namespace Forms.Types
{
    public class UploadEventArgs
    {
        public DateTimeOffset? UploadedTime { get; set; }

        public UploadState State { get; set; }

        public string Id { get; set; }
    }
}