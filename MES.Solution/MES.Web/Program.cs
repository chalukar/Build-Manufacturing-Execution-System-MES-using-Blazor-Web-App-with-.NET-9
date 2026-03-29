using MES.Web;
using MES.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri("https://localhost:5055")
});

builder.Services.AddScoped<WorkOrderService>();
builder.Services.AddScoped<WorkCentreService>();

await builder.Build().RunAsync();
