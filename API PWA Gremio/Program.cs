using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Core.Services.Implementations;
using PWA_GREMIO_API.Core.Services.Interfaces;
using PWA_GREMIO_API.Infraestructure.Data;
using PWA_GREMIO_API.Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using PWA_GREMIO_API.Core.ContextProvider;
using PWA_GREMIO_API.Infraestructure.Repository;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API;
using System;
using PWA_GREMIO_API.Core.Entities.Users;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

var secretKey = builder.Configuration.GetValue<string>("Audience:Secret") ?? "DefaultSecretKey";

if (string.IsNullOrEmpty(secretKey))
{
    throw new ArgumentNullException("Audience:Secret", "The secret key cannot be null or empty. Please check your configuration.");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

//var _myCors = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var signalRConnectionString = builder.Configuration.GetSection("Azure:SignalR:ConnectionString").Value;

// Configura SignalR con Azure SignalR Service
builder.Services.AddSignalR()
    .AddAzureSignalR(options => {

        options.ConnectionString = signalRConnectionString;

        }
    )
    .AddHubOptions<GroupHubService>(options =>
    {
        options.HandshakeTimeout = TimeSpan.FromSeconds(30); // Ajusta según sea necesario
    });

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Audience"),
    ValidAudience = builder.Configuration.GetValue<string>("Jwt:Issuer"),
    ValidateLifetime = true,
    RequireExpirationTime = true,
    IssuerSigningKey = signingKey,
    ValidateIssuerSigningKey = true
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/GremioHubService")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            },
            
        };

        options.TokenValidationParameters = tokenValidationParameters;
    });


builder.Services.AddControllers();

// Cors configuration (commented out as per original code)
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _myCors,
        policy =>
        {
            policy.WithOrigins("https://nice-bay-07b86bc0f.5.azurestaticapps.net")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    
        });
});
*/

builder.Services.AddScoped<IContextProvider, JwtContextProvider>();

builder.Services.AddDbContext<DefaultContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Users"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(Auditable<int?>), typeof(UserAuth));
builder.Services.AddScoped(typeof(Auditable<int?>), typeof(UserSignalR));
builder.Services.AddScoped(typeof(Auditable<int?>), typeof(UserData));
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IUserSignalRDataService, UserSignalRDataService>();

builder.Services.AddScoped<AnnoucementEntity, AnnoucementEntity>();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();


builder.Logging.AddConsole();

var app = builder.Build();



// Esto redirecciona http a https
//app.UseHttpsRedirection();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}



//app.UseCors(_myCors);



app.MapControllers();

app.UseWebSockets();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error."
        }.ToString());
    });
});

app.UseAuthorization();

app.MapHub<GroupHubService>("/GremioHubService");


app.Run();

