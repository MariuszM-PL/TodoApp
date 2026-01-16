# 💻 TodoApp - Desktop Task Manager (.NET MAUI)

Nowoczesna aplikacja desktopowa do zarządzania zadaniami, zbudowana w technologii **.NET MAUI** (WinUI 3). Projekt koncentruje się na przejrzystym interfejsie użytkownika, bezpieczeństwie danych oraz architekturze ułatwiającej rozbudowę.

## 🚀 Kluczowe Rozwiązania Desktopowe

* **🔔 Autorski System Powiadomień In-App**: Własny mechanizm "Toast Notification" z animacjami, zaprojektowany specjalnie pod obszar roboczy pulpitu.
* **🔊 Sygnalizacja Audio**: Integracja dźwiękowa (notification.wav) informująca o nadchodzących terminach bez konieczności patrzenia na aplikację.
* **🖥️ Optymalizacja Okna Windows**: 
    * Automatyczne centrowanie aplikacji przy starcie.
    * Zablokowane minimalne wymiary okna (500x650) dla zachowania czytelności UI.
    * Responsywny układ kafelkowy dostosowany do pracy z myszką.

## ✨ Funkcjonalność
* **🛡️ Bezpieczeństwo**: Rejestracja i logowanie użytkowników z nieodwracalnym haszowaniem haseł (**SHA-256**).
* **🗄️ Lokalna Baza Danych**: Wykorzystanie **SQLite** (asynchroniczne operacje I/O) do trwałego przechowywania zadań.
* **🎨 Personalizacja**: Pełne wsparcie dla motywu **Jasnego** i **Ciemnego** z możliwością wymuszenia konkretnego stylu w ustawieniach.
* **🔍 Filtrowanie i Kategorie**: System kategoryzacji zadań (Dom, Praca, Szkoła itd.) z dynamicznym przypisywaniem kolorów.
* **📅 Terminarz**: Możliwość precyzyjnego planowania daty i godziny realizacji każdego zadania.

## 🛠️ Stack Technologiczny
* **Framework**: .NET 8 (MAUI / Windows Machine)
* **Wzorzec projektowy**: MVVM (CommunityToolkit.Mvvm)
* **Baza danych**: SQLite-net-pcl (lokalny plik `.db`)
* **Audio**: Plugin.Maui.Audio
* **Dokumentacja**: Pełna dokumentacja techniczna XML (<summary>)

## 🏗️ Struktura Projektu
* `Models/` - Modele danych i schematy bazy SQLite.
* `ViewModels/` - Logika biznesowa, obsługa Timerów i komend.
* `Views/` - Definicje interfejsu użytkownika w XAML.
* `Services/` - Serwis bazy danych i menedżer dźwięku.
* `Helpers/` - Logika pomocnicza (np. PasswordHasher).

## 📥 Instrukcja Uruchomienia
1. Sklonuj repozytorium: `git clone https://github.com/TwojLogin/TodoApp.git`
2. Otwórz plik `TodoApp.sln` w **Visual Studio 2022**.
3. Jako cel uruchomienia wybierz **Windows Machine**.
4. Naciśnij **F5**, aby skompilować i uruchomić aplikację.

---
### 👥 Autorzy
* **Mariusz Mikołajczyk**
* **Patrycja Dorszyńska**

*Projekt stworzony na zaliczenie przedmiotu w celach edukacyjnych.*