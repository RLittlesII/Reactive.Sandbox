using System.Collections.Generic;

namespace Removeable
{
    public class CoffeeClientMock : ClientMock<CoffeeDto>, IClient
    {
        public CoffeeClientMock()
        {
            Items = new List<CoffeeDto>
            {
                new CoffeeDto
                {
                    Name = "Arusha",
                    Species = Species.Arabica,
                    Regions = new[] {"Mount Meru, Tanzania", "Papua New Guinea"}
                },
                new CoffeeDto { Name = "Benguet", Species = Species.Arabica, Regions = new[] { Regions.Philippines }},
                new CoffeeDto { Name = "Bergendal", Species = Species.Arabica, Regions = new[] { Regions.Indonesia }},
                new CoffeeDto { Name = "Bernardina", Species = Species.Arabica, Regions = new[] { "El Salvador" }},
                new CoffeeDto
                {
                    Name = "Blue Mountain",
                    Species = Species.Arabica,
                    Regions = new[] { "Blue Mountains, Jamaica", "Kenya", "Hawaii", "Papu New Guinea", Regions.Cameroon }
                },
                new CoffeeDto
                {
                    Name = "Bourbon",
                    Species = Species.Arabica,
                    Regions = new[] { "Reunion", "Rwanda", Regions.LatinAmerica }
                },
                new CoffeeDto { Name = "Catuai", Species = Species.Arabica, Regions = new[] { Regions.LatinAmerica }},
                new CoffeeDto
                {
                    Name = "Catimor", Species = Species.Arabica,
                    Regions = new[] { Regions.LatinAmerica, Regions.Indonesia, Regions.India, Regions.China }
                },
                new CoffeeDto { Name = "Caturra", Species = Species.Arabica, Regions = new[] {Regions.LatinAmerica }},
                new CoffeeDto { Name = "Charrier", Species = Species.Charrieriana, Regions = new[] {Regions.Cameroon }}
            };
        }
    }
}