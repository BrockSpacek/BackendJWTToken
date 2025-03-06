using System.Text;
using BackendJWTToken.Context;
using BackendJWTToken.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
    policy => {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));



var secretKey = builder.Configuration["Jwt:Key"] ?? "superSecretKey@345";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // This is setting up our authentication to know what to expect and check to see if our token is still valid
        // These options are defining what is valid in our token as well, and should correlate to the options that we set upon generating our token
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // This is a list of all the places a token should be allowed to get generated from
        ValidIssuers = new[]
        {
            "backendjwtapibs-dwdcf6hta0f3gdcs.westus-01.azurewebsites.net"
        },
        // This is a list of all the places a token should be allowed to get used
        ValidAudiences = new[]
        {
            "backendjwtapibs-dwdcf6hta0f3gdcs.westus-01.azurewebsites.net"
        },
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Secret key
    };
});

// Now that this is set up, you can make a call to an endpoint that is protected by [Authorize] by adding a "authorization header"




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
