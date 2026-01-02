using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

using simple_crud.Client.Models;
using simple_crud.Library.Models;
using simple_crud.Library.Models.DTOs;

using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace simple_crud.Client.Infrastructure;

public class LabsystecBlogService : IBlogService
{
    readonly HttpClient _httpClient;
    readonly IJSRuntime _jsRuntime;

    public LabsystecBlogService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<OperationResult<PostDto?>> GetPostByTitle(string title)
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
                return new OperationResult<PostDto?>(false, "Sin Token de autorización.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"post/{title}");

            if (!response.IsSuccessStatusCode)
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new OperationResult<PostDto?>(false, $"{problem?.Detail ?? "Error desconocido"}");
            }

            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            return new OperationResult<PostDto?>(true, value: result);
        }
        catch (Exception ex)
        {
            return new OperationResult<PostDto?>(false, "An error occurred during fetching post", exception: ex);
        }
    }

    public async Task<OperationResult<IEnumerable<PostDto>>> GetPostByUserAsync(uint idUser)
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
                return new OperationResult<IEnumerable<PostDto>>(false, "Sin Token de autorización.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"usuario/{idUser}/posts");

            if (!response.IsSuccessStatusCode)
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new OperationResult<IEnumerable<PostDto>>(false, $"{problem?.Detail ?? "Error desconocido"}");
            }

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<PostDto>>();

            return new OperationResult<IEnumerable<PostDto>>(true, value: result);
        }
        catch (Exception ex)
        {
            return new OperationResult<IEnumerable<PostDto>>(false, "An error occurred during fetching post", exception: ex);
        }
    }

    public async Task<OperationResult> LoginAsync(LoginUsuarioDTO dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new OperationResult(false, $"{problem?.Detail ?? "Error desconocido"}");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result is null)
                return new OperationResult(false, "Invalid login response");

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);

            return new OperationResult(true, "Login successful");
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
            var response = await _httpClient.PostAsync("auth/logout", null);

            if (!response.IsSuccessStatusCode)
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new OperationResult(false, $"{problem?.Detail ?? "Error desconocido"}");
            }

            return new OperationResult(true, "Logout successful.");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "An error occurred during logout.", ex);
        }
    }

    public async Task<OperationResult> RegisterAsync(CreateUsuarioDTO createUsuarioDTO)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", createUsuarioDTO);

            if (!response.IsSuccessStatusCode)
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new OperationResult(false, $"{problem?.Detail ?? "Error desconocido"}");
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result is null)
                return new OperationResult(false, "Invalid login response");

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);

            return new OperationResult(true, "Registration successful.");
        }
        catch (Exception ex)
        {
            return new OperationResult(false, ex.Message);
        }
    }
}
