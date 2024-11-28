using CommunityToolkit.Maui.Views;
using GroceryAndroid.Models;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace GroceryAndroid.Components;

public partial class SelectionItem : ContentView
{
    public SelectionItem()
    {
        InitializeComponent();
        // Do not set BindingContext to 'this' here. Let the ListView manage it.
    }

    public string ValueString => Value.ToString();

    public bool IsDecrementButtonEnabled => Value > 0;
    public bool IsIncrementButtonEnabled => Value < 99;

    public Color DecrementButtonColor => Value > 0 ? Color.FromArgb("DF3636") : Colors.LightGray;
    public Color IncrementButtonColor => Value < 99 ? Color.FromArgb("4BA62F") : Colors.LightGray;

    private ushort _value;
    public ushort Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(ValueString));
                OnPropertyChanged(nameof(IsDecrementButtonEnabled));
                OnPropertyChanged(nameof(IsIncrementButtonEnabled));
                OnPropertyChanged(nameof(DecrementButtonColor));
                OnPropertyChanged(nameof(IncrementButtonColor));
            }
        }
    }

    private void decrementBtn_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ListItemDisplayDTO dto && dto.Quantity > 0)
        {
            dto.Quantity--;
            OnPropertyChanged(nameof(dto.Quantity));
        }
    }

    private void incrementBtn_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ListItemDisplayDTO dto)
        {
            dto.Quantity++;
            OnPropertyChanged(nameof(dto.Quantity));
        }
    }
}
