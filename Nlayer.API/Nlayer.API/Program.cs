using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nlayer.API.Middlewares;
using Nlayer.API.ValidationFilter;
using Nlayer.Caching;
using Nlayer.Core.Repositories;
using Nlayer.Core.Services;
using Nlayer.Core.UnitofWorks;
using Nlayer.Repository.Context;
using Nlayer.Repository.Repositories;
using Nlayer.Repository.UnitOfWorks;
using Nlayer.Services.Mapping;
using Nlayer.Services.Services;
using Nlayer.Services.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWorks>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(GenericServices<>));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped(typeof(CategoryIDFilter));

builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddScoped(typeof(PutFilter));

builder.Services.AddScoped(typeof(IProductService),typeof(ProductServiceWithCaching));

builder.Services.AddScoped(typeof(IProductRepository),typeof(ProductRepository));

builder.Services.AddScoped(typeof(ICategoryRepository),typeof(CategoryRepository));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
builder.Services.AddControllers(options=>options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
builder.Services.Configure<ApiBehaviorOptions>(options=>options.SuppressModelStateInvalidFilter=true); //bu ifade yalnýzca api tarafýnda yazýlýr, bunun sebebi api'nin kendine özgü bir filteri olmasý,bununla
//filteri ezebiliyoruz.




builder.Services.AddDbContext<AppDbContext>(contexts =>
{
    contexts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), options => options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name));
});

builder.Services.AddMemoryCache(); //memorycach için kullandýk.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Middleware(); //global exception handler
app.UseAuthorization();

app.MapControllers();

app.Run();
