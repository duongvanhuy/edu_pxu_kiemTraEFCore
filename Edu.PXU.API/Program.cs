using Edu.PXU.API.App.Interface;
using Edu.PXU.API.App;
using Edu.PXU.EntityFECore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Edu.PXU.API;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Edu.PXU.API.Common.MapperHelper;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PXUDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));


//var connectionString = builder.Configuration["ConnectionStrings:Database"];
//builder.Services.AddDbContext<PXUDBContext>(o =>
//    o.UseSqlServer(connectionString, b => b.EnableRetryOnFailure()));
// Add services to the container.
// Add services to the container.

builder.Services.AddIdentity<UserIdentity, Role>(op =>
{
    op.User.RequireUniqueEmail = true;

    op.Password.RequireDigit = true;
    op.Password.RequireLowercase = true;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireUppercase = false;
    op.Password.RequiredLength = 6;
    op.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<PXUDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

#region Extensions

builder.Services
    .AddSingleton<IJwtLib, JwtLib>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IUserIdentityRepository, UserIdentityRepository>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IProductImageRepository, ProductImageRepository>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<IImageRepository, ImageRepository>();


builder.Services
    .AddAutoMapper(typeof(CategoryMapper).Assembly)
    .AddAutoMapper(typeof(ProductMapper).Assembly);
#endregion


#region Swagger

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSignalRSwaggerGen();
        options.EnableAnnotations();
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "EOffice API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
      //  options.SchemaFilter<EnumSchemaFilter>();
    });

builder.Services.AddSwaggerGenNewtonsoftSupport();

#endregion

builder.Services
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    }).AddGoogle(options =>
    {
        options.ClientId = "777566047557-oq9necirs7r043c79i0e88617m18dafe.apps.googleusercontent.com";
        options.ClientSecret = "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";
        options.Scope.Add("profile");
        options.SignInScheme = IdentityConstants.ExternalScheme;
    });

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI()
    .UseReDoc(options => { options.SpecUrl = "/swagger/v1/swagger.json"; })
    .UseRouting()
    .UseCors(
        options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(x => true)
    )
    .UseStaticFiles()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
