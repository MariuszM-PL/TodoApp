using System.Security.Cryptography;
using System.Text;

namespace TodoApp.Helpers
{
    /// <summary>
    /// Klasa statyczna dostarczająca mechanizmów bezpiecznego przechowywania haseł.
    /// Wykorzystuje algorytm skrótu SHA-256 do tworzenia nieodwracalnych cyfrowych odcisków haseł.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Przekształca hasło w formie tekstu jawnego na 64-znakowy ciąg szesnastkowy (hash).
        /// </summary>
        /// <param name="password">Hasło wprowadzone przez użytkownika.</param>
        /// <returns>Zahaszowany ciąg znaków lub pusty ciąg, jeśli wejście było puste.</returns>
        public static string HashPassword(string password)
        {
            // Obsługa pustego hasła zapobiegająca błędom referencji (null)
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            // Inicjalizacja algorytmu SHA-256 (Secure Hash Algorithm 2)
            using (var sha256 = SHA256.Create())
            {
                // Konwersja hasła na tablicę bajtów w kodowaniu UTF-8
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Budowanie końcowego ciągu szesnastkowego (hex string)
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    // "x2" oznacza format szesnastkowy z dwoma znakami na bajt
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}