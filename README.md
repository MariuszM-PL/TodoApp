# 💻 TodoApp - Desktop Task Manager (.NET MAUI)

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-8.0-purple) ![Platform](https://img.shields.io/badge/Platform-Windows-blue) ![License](https://img.shields.io/badge/License-MIT-green)

**Nowoczesna aplikacja desktopowa do zarządzania zadaniami**, zbudowana w technologii **.NET MAUI** (WinUI 3). Projekt koncentruje się na przejrzystym interfejsie użytkownika, pełnej responsywności okna, bezpieczeństwie danych oraz architekturze zgodnej ze wzorcem MVVM.

Aplikacja została zaprojektowana specyficznie pod środowisko Windows, oferując natywne doświadczenia, takie jak obsługa paska tytułu, skalowanie formularzy czy integracja z systemem plików.

## ✨ Funkcjonalności i Możliwości

Aplikacja łączy w sobie cechy klasycznego menedżera zadań z rozwiązaniami dedykowanymi dla aplikacji okienkowych.

### 🖥️ User Experience i Responsywność
* **Adaptacyjny Układ**: Formularze dodawania i edycji zadań inteligentnie dostosowują się do szerokości okna, co gwarantuje czytelność na każdym monitorze.
* **Personalizacja Motywu**: Pełne wsparcie dla trybu **Jasnego** i **Ciemnego** (Dark Mode), ze starannie dobraną paletą kolorów dla kontrolek systemowych (TimePicker, DatePicker).
* **Optymalizacja Okna**: Automatyczne centrowanie aplikacji przy starcie oraz blokada minimalnych wymiarów, aby zapobiec "psuciu się" interfejsu przy zbyt małym oknie.

### 📋 Zarządzanie Zadaniami
* **Zaawansowane Filtrowanie**: System kategorii (Dom, Praca, Szkoła, Zakupy, Inne) pozwalający na szybkie sortowanie obowiązków.
* **Wizualizacja Statusu**: Przejrzyste oznaczanie zadań wykonanych poprzez przekreślenie tekstu i zmianę jego przezroczystości (tytuł, opis, termin).
* **Walidacja Danych**: Zabezpieczenia formularzy (np. wymóg podania tytułu) zapobiegające tworzeniu pustych wpisów.

### 🔔 Powiadomienia i Audio
* **Autorski System "Toast"**: Własny mechanizm powiadomień wyskakujących wewnątrz aplikacji, zaprojektowany z myślą o estetyce WinUI 3.
* **Sygnalizacja Dźwiękowa**: Integracja z systemem audio (`notification.wav`) – aplikacja odtwarza dźwięk w momencie nadejścia terminu zadania, nawet gdy działa w tle.

### 🛡️ Dane i Bezpieczeństwo
* **Lokalna Baza Danych**: Wykorzystanie silnika **SQLite** do trwałego i szybkiego przechowywania danych offline.
* **Bezpieczne Logowanie**: System rejestracji użytkowników wykorzystujący algorytm **SHA-256** do nieodwracalnego haszowania haseł.
* **Izolacja Danych**: Każdy użytkownik ma dostęp wyłącznie do swoich zadań.

## 🛠️ Stack Technologiczny

* **Framework**: .NET 8 (MAUI / Windows Machine)
* **Wzorzec projektowy**: MVVM (CommunityToolkit.Mvvm)
* **Baza danych**: SQLite-net-pcl (lokalny plik `.db`)
* **Audio**: Plugin.Maui.Audio
* **UI**: XAML + FlexLayout/Grid (Responsive Design)

## 🏗️ Struktura Projektu

* `Models/` - Modele danych i schematy bazy SQLite.
* `ViewModels/` - Logika biznesowa, obsługa Timerów i komend (MVVM).
* `Views/` - Definicje interfejsu użytkownika w XAML.
* `Services/` - Serwis bazy danych, menedżer dźwięku, nawigacja.
* `Helpers/` - Logika pomocnicza (np. PasswordHasher, konwertery).

## 📥 Instrukcja Uruchomienia

### Deweloperska (Visual Studio)
1.  Sklonuj repozytorium:
    ```bash
    git clone [https://github.com/TwojLogin/TodoApp.git](https://github.com/TwojLogin/TodoApp.git)
    ```
2.  Otwórz plik `TodoApp.sln` w **Visual Studio 2022**.
3.  Jako cel uruchomienia wybierz **Windows Machine**.
4.  Naciśnij **F5**, aby skompilować i uruchomić aplikację.

### Generowanie pliku .exe (Opcjonalnie)
Aby utworzyć samodzielny plik wykonywalny (niewymagający instalacji), użyj terminala w folderze projektu:
```powershell
dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true -p:RuntimeIdentifierOverride=win10-x64
```
### 👥 Autorzy

* **Mariusz Mikołajczyk**
* **Patrycja Dorszyńska**

*Projekt zrealizowany w celach edukacyjnych na zaliczenie przedmiotu.*



