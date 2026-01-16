# 💻 TodoApp - Desktop Task Manager (.NET MAUI)

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-8.0-purple) ![Platform](https://img.shields.io/badge/Platform-Windows-blue) ![License](https://img.shields.io/badge/License-MIT-green)

**Nowoczesna aplikacja desktopowa do zarządzania zadaniami**, zbudowana w technologii **.NET MAUI** (WinUI 3). Projekt koncentruje się na przejrzystym interfejsie użytkownika, responsywności (skalowanie okna), bezpieczeństwie danych oraz architekturze zgodnej ze wzorcem MVVM.

## 🚀 Kluczowe Rozwiązania Desktopowe

* **🖥️ Pełna Responsywność**: Formularze dodawania i edycji zadań automatycznie dostosowują układ (pionowy/poziomy) do szerokości okna, zachowując czytelność na każdym monitorze.
* **🔔 Autorski System Powiadomień**: Własny mechanizm "Toast Notification" z animacjami, zaprojektowany specjalnie pod obszar roboczy pulpitu Windows.
* **🔊 Sygnalizacja Audio**: Integracja dźwiękowa (`notification.wav`) informująca o nadchodzących terminach w czasie rzeczywistym.
* **🪟 Optymalizacja Okna**:
    * Automatyczne centrowanie aplikacji przy starcie.
    * Zablokowane minimalne wymiary dla zachowania spójności UI.
    * Generowanie bezpośredniego pliku `.exe` (Windows App SDK Self-Contained).

## ✨ Funkcjonalność

* **🛡️ Bezpieczeństwo**: Rejestracja i logowanie użytkowników z nieodwracalnym haszowaniem haseł (**SHA-256**).
* **🗄️ Lokalna Baza Danych**: Wykorzystanie **SQLite** (asynchroniczne operacje I/O) do trwałego przechowywania zadań i ustawień.
* **🎨 Personalizacja**: Pełne wsparcie dla motywu **Jasnego** i **Ciemnego** (z poprawioną czytelnością kontrolek systemowych TimePicker/DatePicker).
* **🔍 Filtrowanie i Kategorie**: System kategoryzacji zadań (Dom, Praca, Szkoła itp.) z dynamicznym kolorowaniem etykiet.
* **✅ Status Zadań**: Wizualne przekreślanie i wyszarzanie ukończonych zadań (tytuł, opis, termin).

## 🛠️ Stack Technologiczny

* **Framework**: .NET 8 (MAUI / Windows Machine)
* **Wzorzec projektowy**: MVVM (CommunityToolkit.Mvvm)
* **Baza danych**: SQLite-net-pcl (lokalny plik `.db`)
* **Audio**: Plugin.Maui.Audio
* **UI**: XAML + FlexLayout/Grid dla responsywności

## 🏗️ Struktura Projektu

* `Models/` - Modele danych i schematy bazy SQLite.
* `ViewModels/` - Logika biznesowa, obsługa Timerów i komend (MVVM).
* `Views/` - Definicje interfejsu użytkownika w XAML.
* `Services/` - Serwis bazy danych, menedżer dźwięku, nawigacja.
* `Helpers/` - Logika pomocnicza (np. PasswordHasher, konwertery).

## 📥 Instrukcja Uruchomienia

1.  Sklonuj repozytorium:
    ```bash
    git clone [https://github.com/TwojLogin/TodoApp.git](https://github.com/TwojLogin/TodoApp.git)
    ```
2.  Otwórz plik `TodoApp.sln` w **Visual Studio 2022**.
3.  Jako cel uruchomienia wybierz **Windows Machine**.
4.  Naciśnij **F5**, aby skompilować i uruchomić aplikację.

> **Uwaga:** Aplikacja generuje plik wykonywalny `.exe` w trybie *Self-Contained*, co oznacza, że nie wymaga od użytkownika końcowego instalowania dodatkowych bibliotek Windows App SDK.

---

### 👥 Autorzy

* **Mariusz Mikołajczyk**
* **Patrycja Dorszyńska**

*Projekt zrealizowany w celach edukacyjnych na zaliczenie przedmiotu.*
