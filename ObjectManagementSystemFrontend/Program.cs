using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ObjectManagementSystemFrontend;
using ObjectManagementSystemFrontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

var filename = $"appsettings.{builder.HostEnvironment.Environment}.json";

using var configuration = await httpClient.GetAsync(filename);

using var stream = await configuration.Content.ReadAsStreamAsync();

builder.Configuration.AddJsonStream(stream);

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped<DataProviderService>();
builder.Services.AddScoped<StateManagerService>();
builder.Services.AddScoped<DataPersistenceService>();

await builder.Build().RunAsync();