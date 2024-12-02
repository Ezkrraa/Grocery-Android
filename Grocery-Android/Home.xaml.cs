using CommunityToolkit.Maui.Views;
using GroceryAndroid.Components;
using GroceryAndroid.Models;
using GroceryAndroid.Networking;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace GroceryAndroid;

public partial class Home : ContentPage, INotifyPropertyChanged
{
    private ObservableCollection<ListItemDisplayDTO> _items;
    public ObservableCollection<ListItemDisplayDTO> Items
    {
        get => _items;
        set
        {
            if (_items != value)
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public Home()
    {
        InitializeComponent();
        BindingContext = this;

        // Initialize collection with sample data
        Loaded += async (s, e) =>
        {
            GroceryListController controller = new();
            IEnumerable<CategoryDisplayDTO>? categories = await controller.GetAllCategories();
            if (categories != null)
            {
                Guid listId = Guid.NewGuid();
                CategoryListDTO? catItems = await controller.GetItemsInCategory(categories.First().Id);
                if (catItems != null)
                {
                    Items = new(catItems.Items.Select(item => new ListItemDisplayDTO(listId, item.Name, 0, categories.First().Id)));
                }
            }
            else
            {
                Items = new(Enumerable.Range(1, 20).Select(i => new ListItemDisplayDTO(Guid.NewGuid(), $"Item number {i}", 0, Guid.NewGuid())));
            }
        };
    }
    private void decrementBtn_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ListItemDisplayDTO dto && dto.Quantity > 0)
        {
            dto.Quantity--;
            // Notify property changes if necessary (depends on your implementation)
            dto.UpdateQuantity();
        }
    }

    private void incrementBtn_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ListItemDisplayDTO dto)
        {
            dto.Quantity++;
            // Notify property changes if necessary (depends on your implementation)
            dto.UpdateQuantity();
        }
    }

    protected override bool OnBackButtonPressed()
    {
        AuthController.ClearToken();
        return base.OnBackButtonPressed();
    }
}


//Items = new (Enumerable.Range(1, 30).Select(i => new ListItemDisplayDTO(Guid.NewGuid(), "Name", 0, Guid.NewGuid())));
/*
GroceryListController controller = new();
IEnumerable<CategoryDisplayDTO>? categories = await controller.GetAllCategories() ?? throw new Exception("Couldn't get categories");
if (categories == null)
    EmptyNotifLabel.IsVisible = true;
else
{
    Category = categories.First();
    Guid listId = Guid.NewGuid();
    CategoryListDTO? catItems = await controller.GetItemsInCategory(Category.Id);
    if (catItems == null)
        EmptyNotifLabel.Text = "No items in category :(";
    else
    {
        Items = new(catItems.Items.Select(item => new ListItemDisplayDTO(listId, item.Name, 0, Category.Id)));
        EmptyNotifLabel.Text = catItems.CategoryName;
        foreach (ItemDisplayDTO item in catItems.Items)
        {
            EmptyNotifLabel.Text += "\n\t\t\t➥" + item.Name;
        }
    }
    EmptyNotifLabel.IsVisible = true;
}
Category = categories.First();
 */
//        };
//    }
//}