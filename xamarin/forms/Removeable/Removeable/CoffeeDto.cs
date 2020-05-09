using System.Collections.Generic;

namespace Removeable
{
    public class CoffeeDto : Dto
    {
        public string Name { get; set; }

        public string Species { get; set; }

        public IEnumerable<string> Regions { get; set; }
    }
}