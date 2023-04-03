using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CRUD.Data;
using CRUD.Data.Abstract;
using CRUD.Models;

var builder = WebApplication.CreateBuilder(args);

//var optionsBuilder = new DbContextOptionsBuilder<EFDbContext>();
//optionsBuilder.UseSqlServer((builder.Configuration.GetConnectionString("DbContext") ?? throw new InvalidOperationException("Connection string 'DbContext' not found.")));

//EFDbContext dbContext = new EFDbContext(optionsBuilder.Options);

//builder.Services.AddTransient<EFDbContext>();


builder.Services.AddDbContext<EFDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext") ?? throw new InvalidOperationException("Connection string 'DbContext' not found.")));

builder.Services.AddScoped<IRepository, EFRepository>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");
    app.Run();