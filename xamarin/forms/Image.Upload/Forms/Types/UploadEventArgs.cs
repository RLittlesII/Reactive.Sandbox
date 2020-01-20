﻿using System;

namespace Forms.Services
{
    public class UploadEventArgs
    {
        public DateTimeOffset? UploadedTime { get; set; }

        public UploadState State { get; set; }

        public int Id { get; set; }
    }
}