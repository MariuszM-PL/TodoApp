using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// Klasa pomocnicza reprezentująca pojedynczą opcję filtra (np. "Dom", "Praca").
    /// Wykorzystywana do zarządzania stanem zaznaczenia przycisków kategorii na ekranie głównym.
    /// </summary>
    public partial class FilterOption : ObservableObject
    {
        /// <summary>
        /// Nazwa kategorii wyświetlana na przycisku filtra.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Określa, czy dany filtr jest aktualnie aktywny. 
        /// Zmiana tej właściwości automatycznie aktualizuje wygląd przycisku w UI (XAML Triggers).
        /// </summary>
        [ObservableProperty]
        bool isSelected;
    }
}