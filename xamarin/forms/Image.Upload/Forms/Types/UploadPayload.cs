using System.Collections.Generic;

namespace Forms.Types
{
    public class UploadPayload
    {
        public Form Form { get; set; }
        public IEnumerable<Image> Images {get;set;}
     }
}