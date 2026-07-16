using System.IdentityModel.Tokens.Jwt;
using CleanAuth.Application;
using CleanAuth.Infrastructure;
using CleanAuth.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// we disable the default mapping of claims to Microsoft-specific claim types, so that we can use the standard claim types in our application.
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Wire up our two layers via the extension methods we wrote
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add the JwtSettings to the DI container and bind it to the configuration section "JwtSettings"
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
?? throw new InvalidOperationException("JwtSettings section is missing in the configuration.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret)),
    };
});

// Add authorization services to the DI container
// This allows us to use the [Authorize] attribute in our controllers to protect endpoints and require authentication.
builder.Services.AddAuthorization();

// build our application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();  // must come before UseAuthorization, otherwise the authorization will not work correctly. 
app.UseAuthorization();

app.MapControllers();


// in the last step we run the application, which starts listening for incoming HTTP requests and handles them according to the configured middleware and routing.
app.Run();

