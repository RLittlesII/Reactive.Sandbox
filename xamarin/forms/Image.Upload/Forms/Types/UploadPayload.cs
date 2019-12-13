using System.Collections.Generic;

namespace Forms.Types
{
    public class UploadPayload
    {
        public int Id { get; set; }
        public Form Form { get; set; }
        public IEnumerable<Image> Images {get;set;}
     }
}