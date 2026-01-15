using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.ViewModels
{
    // Klasa pomocnicza do obsługi wyglądu przycisków filtrów
    public partial class FilterOption : ObservableObject
    {
        public string Name { get; set; }

        [ObservableProperty]
        bool isSelected;
    }
}