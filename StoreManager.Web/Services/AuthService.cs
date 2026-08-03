using System.Net.Http.Json;

namespace StoreManager.Web.Services;

public class LoginResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class AuthService(HttpClient http)
{
    public LoginResult? CurrentUser { get; private set; }
    public event Action? OnChange;

    public async Task<(bool Success, string? ErrorMessage)> LoginAsync(string email, string password)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", new { email, password });

        if (response.IsSuccessStatusCode)
        {
            CurrentUser = await response.Content.ReadFromJsonAsync<LoginResult>();
            OnChange?.Invoke();
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(error) ? "Invalid email or password" : error);
    }

    public void Logout()
    {
        CurrentUser = null;
        OnChange?.Invoke();
    }

    public bool IsLoggedIn => CurrentUser != null;
}