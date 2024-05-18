using Microsoft.AspNetCore.Http.Connections;
using WebApplicationSignal.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
}).AddAzureSignalR("connection_string"); ;


builder.Services.AddLogging(logging =>
{
    // Configura el logging según tus necesidades
    logging.AddConsole(); // Agrega el logging a la consola
    logging.AddDebug();   // Agrega el logging a Debug
                          // Puedes agregar otros proveedores de logging aquí, como ser log4net, Serilog, etc.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
});
app.Run();
