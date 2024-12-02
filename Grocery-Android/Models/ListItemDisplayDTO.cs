using System.ComponentModel;

namespace GroceryAndroid.Models;

public class ListItemDisplayDTO : INotifyPropertyChanged
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryId { get; set; }

    public ListItemDisplayDTO(Guid id, string name, int quantity, Guid categoryId)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        CategoryId = categoryId;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void UpdateQuantity()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
    }
}
