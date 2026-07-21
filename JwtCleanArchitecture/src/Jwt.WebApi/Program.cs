
using System.Text;
using Jwt.Application.Interfaces;
using Jwt.Infrastructure.Persistence;
using Jwt.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// add or services here
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWT Clean Architecture API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,      // was ApiKey — Http is correct for Bearer
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token. Example: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    // v10: this is now a delegate that receives the built document
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});



// Connect to Client side
// 1. Register the CORS service — put this near your other builder.Services.Add... calls
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
        policy.WithOrigins("http://localhost:5002")   // <-- the exact origin Jwt.Client runs on
              .AllowAnyHeader()
              .AllowAnyMethod());
});



// now add the db here
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
.EnableSensitiveDataLogging()
.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

// now we add the Interfaces here
builder.Services.AddScoped<ITokenServices, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();



// jwt Authentication
var jwtSetting = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSetting["SecretKey"]
?? throw new InvalidOperationException("Key is missing in Configurstions");


// now we add the authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSetting["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSetting["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero

    };
});


// now add the authorization
builder.Services.AddAuthorization();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("BlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}


app.Run();