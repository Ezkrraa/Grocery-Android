namespace GroceryAndroid.Models;

public class AddToListItemDTO
{
    public Guid ItemId { get; set; }
    public Guid ListId { get; set; }
    public ushort Quantity { get; set; }

    public AddToListItemDTO(Guid itemId, Guid listId, ushort quantity)
    {
        ItemId = itemId;
        ListId = listId;
        Quantity = quantity;
    }
}
