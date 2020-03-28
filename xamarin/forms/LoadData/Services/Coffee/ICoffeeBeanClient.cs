using System;

namespace Services.Coffee
{
    public interface ICoffeeBeanClient : IHubClient
    {
        IObservable<CoffeeBeanDto> CoffeeBeans { get; }
    }
}