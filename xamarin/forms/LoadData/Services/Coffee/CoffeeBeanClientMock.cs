using System;
using System.Reactive.Linq;

namespace Services.Coffee
{
    public class CoffeeBeanClientMock : HubClientMock, ICoffeeBeanClient
    {
        public CoffeeBeanClientMock()
        {
            CoffeeBeans =
                Observable
                    .Interval(TimeSpan.FromSeconds(1.5))
                    .Select(x => new CoffeeBeanDto
                    {
                        Pounds = 2500,
                        Type = "Arabica",
                        Name = "Mr. Bean"
                    });
        }

        public IObservable<CoffeeBeanDto> CoffeeBeans { get; }
    }
}