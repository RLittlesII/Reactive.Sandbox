using Forms.Data;
using Forms.Types;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Services
{
    public class FormsService : IFormsService
    {
        private readonly ICache _cache;
        public FormsService(ICache cache = null)
        {
            _cache = cache ?? Locator.Current.GetService<ICache>();
        }
        public async Task<IEnumerable<UploadPayload>> GetPayloads()
        {
            var payloads = await _cache.Get<IEnumerable<UploadPayload>>("Payloads");
            return payloads;
        }

    }
}
