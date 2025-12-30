using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace simple_crud.Client.Infrastructure;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    readonly IJSRuntime _jsRuntime;

    public JwtAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync() 
    { 
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken"); 
        
        if (string.IsNullOrWhiteSpace(token)) 
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); 
        
        var handler = new JwtSecurityTokenHandler(); 
        var jwt = handler.ReadJwtToken(token); 
        
        var identity = new ClaimsIdentity(jwt.Claims, "jwtAuth"); 
        var user = new ClaimsPrincipal(identity); 
        return new AuthenticationState(user); 
    }

    public void NotifyUserAuthentication() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); 
    
    public void NotifyUserLogout() => NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
}
