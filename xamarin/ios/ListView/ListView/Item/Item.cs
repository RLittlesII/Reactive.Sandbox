using System;

namespace ListView
{
    public class Item
    {
        public Item()
        {
            Id = Guid.NewGuid();
            Type = ItemType.Some;
        }

        public Guid Id { get; set; }

        public ItemType Type { get; set; }

        public bool IsToggled { get; set; }
    }
}