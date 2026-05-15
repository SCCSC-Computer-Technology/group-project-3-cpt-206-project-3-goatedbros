namespace PantryPlatoonMVCMain.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public int TotalItems => Items.Sum(x => x.Quantity);

        public decimal TotalWeight => Items.Sum(x => x.Weight * x.Quantity);

        public void AddItem(Item item, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(x => x.ItemId == item.ItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    CategoryName = item.ItemCategory?.ItemCategoryName ?? "Unknown",
                    Weight = item.Weight ?? 0,
                    Points = item.Points ?? 0,
                    Quantity = quantity,
                    ImagePath = item.ImagePath
                });
            }
        }

        public void RemoveItem(int itemId)
        {
            Items.RemoveAll(x => x.ItemId == itemId);
        }

        public void UpdateQuantity(int itemId, int quantity)
        {
            var item = Items.FirstOrDefault(x => x.ItemId == itemId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(itemId);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}