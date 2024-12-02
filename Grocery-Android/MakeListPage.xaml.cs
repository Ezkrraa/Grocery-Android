using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using GroceryAndroid.Components;
using GroceryAndroid.Models;
using GroceryAndroid.Networking;

namespace GroceryAndroid;

public partial class MakeListPage : ContentPage, INotifyPropertyChanged
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

    public MakeListPage()
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
                // make a new grocery list
                Guid listId = Guid.NewGuid();
                CategoryListDTO? catItems = await controller.GetItemsInCategory(
                    categories.First().Id
                );

                if (catItems != null)
                {
                    Items = new(
                        catItems.Items.Select(item => new ListItemDisplayDTO(
                            listId,
                            item.Name,
                            0,
                            categories.First().Id
                        ))
                    );
                }
            }
            else
            {
                Items = new(
                    Enumerable
                        .Range(1, 20)
                        .Select(i => new ListItemDisplayDTO(
                            Guid.NewGuid(),
                            $"Item number {i}",
                            0,
                            Guid.NewGuid()
                        ))
                );
            }
        };
    }

    private void decrementBtn_Clicked(object sender, EventArgs e)
    {
        if (
            sender is Button button
            && button.BindingContext is ListItemDisplayDTO dto
            && dto.Quantity > 0
        )
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
            if (dto.Quantity < 99)
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
