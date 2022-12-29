using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseWebRoot("wwwroot").UseStaticWebAssets();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Host.UseSerilog((context, configuration) =>
{

    configuration
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.File(@"C:\Application\Web\BET\Logs\" + DateTime.Now.ToString("yyyyMMdd") + @"\core.api.log", rollingInterval: RollingInterval.Hour, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{ClientIp}] [{RequestId}] [{RequestPath}] [{Message:lj}] [{Exception}]{NewLine}");
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

DefaultFilesOptions options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("Authentication/Account/Login");
app.UseDefaultFiles(options);
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                    name: "Authentication",
                    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Dashboard}/{id?}");
});

app.MapRazorPages();
app.Run();
