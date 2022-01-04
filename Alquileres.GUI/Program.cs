using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

/// <summary>
/// Uso de carpeta para propietarios
/// </summary>
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        String.Concat(Environment.CurrentDirectory, "\\", "Files", "\\", "Owner")),
    RequestPath = "/Owner"
});

/// <summary>
/// Uso de carpeta para archivos locales
/// </summary>
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        String.Concat(Environment.CurrentDirectory, "\\", "Files", "\\", "LocalFiles")),
    RequestPath = "/LocalFiles"
});

/// <summary>
/// Uso de carpeta para propiedades
/// </summary>
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        String.Concat(Environment.CurrentDirectory, "\\", "Files", "\\", "Property")),
    RequestPath = "/Property"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
