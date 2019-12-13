using System.Collections.Generic;
using System.Threading.Tasks;
using Forms.Types;

namespace Forms.Services
{
    public interface IFormsService
    {
        Task<IEnumerable<UploadPayload>> GetPayloads();
    }
}