using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyContactCloud.Client;
using MyContactCloud.Client.Services;
using MyContactCloud.Client.Services.Interfaces;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// add HttpClient as a service
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ICategoryDTOService, WASMCategoryDTOService>();
builder.Services.AddScoped<IContactDTOService, WASMContactDTOService>();

await builder.Build().RunAsync();
