using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nlayer.Caching;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitofWorks;
using Nlayer.Repository.Context;
using Nlayer.Repository.Repositories;
using Nlayer.Repository.UnitOfWorks;
using Nlayer.Services.Mapping;
using Nlayer.Services.Services;
using NLayer.WEB.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<ProductApiService>(opt => {
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);

});
builder.Services.AddHttpClient<CategoryApiService>(opt => {
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);

});

builder.Services.AddDbContext<AppDbContext>(contexts =>
{
    contexts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), options => options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name));
});

builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductServiceWithCaching));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWorks));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryServiceWithCaching));
builder.Services.AddScoped(typeof(ICategoryRepository),typeof(CategoryRepository));
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
app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
app.UseAuthorization();

app.MapRazorPages();

app.Run();
