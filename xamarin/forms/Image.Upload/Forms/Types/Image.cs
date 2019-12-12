using System;
using System.Collections.Generic;
using System.Text;

namespace Forms.Types
{
    public class Image
    {
        public int Id { get; set; }
        public string FileLocation { get; set; }
        public DateTime? UploadedDate { get; set; }
    }
}
