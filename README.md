# 📝 TodoApp - .NET MAUI Task Manager

Nowoczesna aplikacja desktopowa i mobilna do zarządzania zadaniami, zbudowana w technologii **.NET MAUI** z naciskiem na architekturę MVVM oraz bezpieczeństwo danych.

## 🚀 Najnowsze Funkcje (Wersja 1.1)

* **🔔 Autorski System Powiadomień In-App**: Własny mechanizm "Toast Notification" z animacjami, który przypomina o nadchodzących terminach w czasie rzeczywistym.
* **🔊 Powiadomienia Dźwiękowe**: Integracja audio informująca użytkownika o zbliżającym się terminie zadania.
* **🛡️ Zaawansowane Bezpieczeństwo**: Pełne haszowanie haseł algorytmem **SHA-256** przed zapisem do bazy danych.
* **🖥️ Desktop Optimization**: Mechanizm automatycznego centrowania okna i blokada minimalnych wymiarów dla systemów Windows.

## ✨ Standardowe Funkcje
* **🗄️ Lokalna Baza Danych**: Wykorzystanie **SQLite** z asynchronicznym dostępem do danych.
* **🎨 Personalizacja**: Dynamiczna zmiana motywów (Jasny/Ciemny/Systemowy) działająca w czasie rzeczywistym.
* **🔍 Wyszukiwanie i Filtry**: Zaawansowane filtrowanie po nazwie, opisie oraz kategoriach.
* **🏷️ System Kategorii**: Dynamiczne przypisywanie kolorów do kategorii (Dom, Praca, Szkoła, Zakupy).
* **📅 Planowanie**: Precyzyjne ustawianie daty i godziny zakończenia zadania.

## 🛠️ Stack Technologiczny
* **Framework**: .NET 8 (MAUI)
* **Wzorzec projektowy**: MVVM (CommunityToolkit.Mvvm)
* **Baza danych**: SQLite-net-pcl
* **Audio**: Plugin.Maui.Audio
* **Dokumentacja**: Pełna dokumentacja XML zgodna ze standardami C#



## 🏗️ Architektura Projektu
Projekt został podzielony zgodnie z dobrymi praktykami programistycznymi:
* `Models/` - Definicje danych i tabele bazy danych.
* `ViewModels/` - Logika biznesowa i obsługa zdarzeń.
* `Views/` - Warstwa interfejsu (XAML).
* `Services/` - Zarządzanie bazą danych i logiką audio.
* `Helpers/` - Narzędzia pomocnicze (haszowanie haseł).

## 📥 Uruchomienie projektu
1. Sklonuj repozytorium: `git clone https://github.com/TwojLogin/TodoApp.git`
2. Otwórz plik `TodoApp.sln` w **Visual Studio 2022**.
3. Upewnij się, że masz zainstalowane obciążenie (workload) `.NET MAUI`.
4. Wybierz cel `Windows Machine` lub `Android Emulator` i uruchom projekt (F5).

---
### 👥 Autorzy
* **Mariusz Mikołajczyk**
* **Patrycja Dorszyńska**

*Projekt stworzony w celach edukacyjnych na zaliczenie przedmiotu.*