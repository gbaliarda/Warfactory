public interface IInventory
{
    ItemStack AddItemStack(ItemStack newStack);
    ItemStack AddItem(Item item, int amount = 1);
    bool RemoveItem(Item item, int amount = 1);
    bool RemoveItemStack(ItemStack stack);
    int GetItemAmount(Item item);

    bool HasItem(Item item) => GetItemAmount(item) > 0;
}
