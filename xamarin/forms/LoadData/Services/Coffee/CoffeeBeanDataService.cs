using System;
using System.Reactive;
using DynamicData;

namespace Services.Coffee
{
    public class CoffeeBeanDataService : DataServiceBase<CoffeeBeanDto>, ICoffeeBeanDataService
    {
        private readonly ICoffeeBeanClient _hubClient;

        public CoffeeBeanDataService(ICoffeeBeanClient hubClient)
            : base(hubClient)
        {
            _hubClient = hubClient;
            _hubClient
                .CoffeeBeans
                .Subscribe(x => SourceCache.AddOrUpdate(x));
        }
    }
}