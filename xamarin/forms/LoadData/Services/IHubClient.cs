using System;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IHubClient
    {
        Task Connect();

        Task InvokeAsync(string method);
    }
}