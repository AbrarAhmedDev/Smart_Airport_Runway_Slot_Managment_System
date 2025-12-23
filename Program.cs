using BlazorApp1;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1.Services; // Ensure this is here

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// --- MANDATORY REQUIREMENT: DEPENDENCY INJECTION ---
// We register both services here so they can be injected into any component
builder.Services.AddSingleton<SlotService>();
builder.Services.AddSingleton<AirportService>();

await builder.Build().RunAsync();