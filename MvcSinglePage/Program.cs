using Microsoft.EntityFrameworkCore;
using MvcSinglePage.ApplicationServices.Services;
using MvcSinglePage.ApplicationServices.Services.Contracts;
using MvcSinglePage.Models;
using MvcSinglePage.Models.Services.Contracts;
using MvcSinglePage.Models.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SinglePageDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("SinglePageConnection")
        ));



//Add Scoped
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductApplicationService, ProductApplicationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
