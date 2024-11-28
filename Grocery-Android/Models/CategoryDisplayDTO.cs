namespace GroceryAndroid.Models;
public record CategoryDisplayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CategoryDisplayDTO(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public CategoryDisplayDTO() { }
}
