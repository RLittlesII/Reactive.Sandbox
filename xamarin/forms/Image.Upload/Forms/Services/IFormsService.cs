using System.Collections.Generic;
using System.Threading.Tasks;
using Forms.Types;

namespace Forms.Services
{
    public interface IFormsService
    {
        Task<IEnumerable<UploadPayload>> GetPayloads();
        Task InsertPayload(UploadPayload payload);
        Task InvalidateAllPayloads();
    }
}