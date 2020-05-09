namespace Removeable
{
    public class CoffeeDataService : DataServiceBase<CoffeeDto>, ICoffeeDataService
    {
        public CoffeeDataService(IClient client)
            : base(client)
        {
        }
    }
}