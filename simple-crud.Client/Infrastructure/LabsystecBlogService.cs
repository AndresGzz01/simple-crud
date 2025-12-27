using simple_crud.Client.Models;

using simple_crud.Library.Models;
using simple_crud.Library.Models.DTOs;

using System.Net.Http.Json;

namespace simple_crud.Client.Infrastructure;

public class LabsystecBlogService : IBlogService
{
    readonly HttpClient _httpClient;

    public LabsystecBlogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OperationResult> LoginAsync(LoginUsuarioDTO loginUsuarioDTO)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginUsuarioDTO);
         
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new OperationResult(false, $"Login failed: {errorMessage}");
            }

            return new OperationResult(true, "Login successful.");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "An error occurred during login.", ex);
        }
    }

    public async Task<OperationResult> LogoutAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/auth/logout", null);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new OperationResult(false, $"Logout failed: {errorMessage}");
            
            }
            return new OperationResult(true, "Logout successful.");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "An error occurred during logout.", ex);
        }
    }
}
