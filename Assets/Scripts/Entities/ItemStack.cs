
public class ItemStack
{
    public Item Item { get; protected set; }
    public int Amount { get; protected set; }

    public ItemStack(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void IncreaseAmount(int amount)
    {
        if (Amount + amount <= Item.MaxStackSize)
        { 
            Amount += amount;
        }
    }

    public void DecreaseAmount(int amount) {
        Amount -= amount;
    }


    public ItemStack Clone()
    {
        return new ItemStack(Item, Amount);
    }
}
