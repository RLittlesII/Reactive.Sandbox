using System.Reactive;
using DynamicData;

namespace Services.Coffee
{
    public class CoffeeBeanDataService : DataServiceBase<CoffeeBeanDto>, IDataService<CoffeeBeanDto>
    {
        public CoffeeBeanDataService(ICoffeeBeanClient hubClient)
            : base(hubClient)
        {
            HubClient
                .CoffeeBeans
                .Subscribe(x => SourceCache.AddOrUpdate(x));
        }
    }
}