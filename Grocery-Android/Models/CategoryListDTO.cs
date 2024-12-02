namespace GroceryAndroid.Models;

public class CategoryListDTO
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public ICollection<ItemDisplayDTO> Items { get; set; }

    public CategoryListDTO(Guid id, string catName, ICollection<ItemDisplayDTO> items)
    {
        Id = id;
        CategoryName = catName;
        Items = items;
    }

    public CategoryListDTO() { }
}
