using System.Collections.Generic;
using System.Linq;

namespace Forms.Types
{
    public class UploadPayload
    {
        public UploadPayload()
        {
            Form = new Form();
            Images = Enumerable.Empty<Image>();
        }

        public string Id { get; set; }
        public Form Form { get; set; }
        public IEnumerable<Image> Images {get;set;}
     }
}