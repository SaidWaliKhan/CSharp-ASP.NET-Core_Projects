using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Common.Interfaces;
using ProductCatalog.Application.Products.Interfaces;
using ProductCatalog.Infrastructure.Persistence;
using ProductCatalog.Infrastructure.Persistence.Repositories;
using ProductCatalog.Infrastructure.Services;
using MediatR;
using System.Reflection;
using FluentValidation;
using ProductCatalog.Application.Common.Behaviors;
using ProductCatalog.API.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();



// add the db here
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// add the service
builder.Services.AddScoped<IProductService, ProductService>();


// add the repository here
builder.Services.AddScoped<IProductRepository, ProductRepository>();



// add the mediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        Assembly.Load("ProductCatalog.Application"));
});


// reister the validation
builder.Services.AddValidatorsFromAssembly(
    Assembly.Load("ProductCatalog.Application"));

// reg the ipipline behavior
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));


// add the redis here cache here 
builder.Services.AddStackExchangeRedisCache(options =>
{
   options.Configuration =
       builder.Configuration["Redis:ConnectionString"];
});

// add the password 
builder.Services.AddScoped<PasswordHasher<User>>();
var app = builder.Build();



// service for jwt
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
        };
});
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.Run();
