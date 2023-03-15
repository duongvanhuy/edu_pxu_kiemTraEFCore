using Edu.PXU.API.App.Interface;
using Edu.PXU.API.App;
using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PXUDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));


//var connectionString = builder.Configuration["ConnectionStrings:Database"];
//builder.Services.AddDbContext<PXUDBContext>(o =>
//    o.UseSqlServer(connectionString, b => b.EnableRetryOnFailure()));
// Add services to the container.
// Add services to the container.
builder.Services.AddControllersWithViews();

#region Extensions

builder.Services
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IProductImageRepository, ProductImageRepository>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<IImageRepository, ImageRepository>();
  

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
