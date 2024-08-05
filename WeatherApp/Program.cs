using Microsoft.EntityFrameworkCore;
using WeatherApp.Data;
using WeatherApp.Service;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("WeatherDbContext");

// Register the DbContext with the dependency injection container
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddHostedService<WeatherUpdateHostedService>();
builder.Services.AddTransient<IWeatherServiceBusiness, WeatherServiceBusiness>();
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
