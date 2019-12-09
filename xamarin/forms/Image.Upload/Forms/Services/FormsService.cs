using Forms.Data;
using Forms.Types;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Services
{
    public class FormsService : IFormsService
    {
        private const string PAYLOADS = "Payloads";
        private readonly ICache _cache;
        public FormsService(ICache cache = null)
        {
            _cache = cache ?? Locator.Current.GetService<ICache>();
        }
        public async Task<IEnumerable<UploadPayload>> GetPayloads()
        {
            var payloads = await _cache.Get<IEnumerable<UploadPayload>>(PAYLOADS);
            return payloads;
        }
        public async Task InsertPayload(UploadPayload payload)
        {
            var cachedPayloads = await GetPayloads();

            if (cachedPayloads == null)
            {
                var payloads = new List<UploadPayload>();
                payloads.Add(payload);
                await _cache.Insert<IEnumerable<UploadPayload>>(PAYLOADS, payloads);
            }
            else
            {
                var payloadsToInsert = cachedPayloads.ToList();
                payloadsToInsert.Add(payload);
                await _cache.Insert<IEnumerable<UploadPayload>>(PAYLOADS, payloadsToInsert);
            }                
        }
        public async Task InvalidateAllPayloads()
        {
            await _cache.Invalidate(PAYLOADS);
        }
    }
}
