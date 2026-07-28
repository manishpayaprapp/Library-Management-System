using LibraryManagementSystem.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://127.0.0.1:5100");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Database & Library services
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<LibraryService>();
builder.Services.AddScoped<AuthService>();

// Keep old BookService for backward compat if needed
// builder.Services.AddSingleton<BookService>(); // no longer needed

var app = builder.Build();

// Initialize DB schema and seed data
using (var scope = app.Services.CreateScope())
{
    var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
    await dbService.InitializeSchemaAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
