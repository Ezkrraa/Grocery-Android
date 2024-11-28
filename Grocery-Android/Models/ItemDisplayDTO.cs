namespace GroceryAndroid.Models;

public class ItemDisplayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ItemDisplayDTO(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
