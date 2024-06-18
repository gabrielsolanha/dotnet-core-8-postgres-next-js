using AplicacaoWeb.Data.Context;
using AplicacaoWeb.Dacpac;
using AplicacaoWeb.IoC;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AplicacaoWeb.Domain;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
DependencyInjector.Register(builder.Services);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });


});

string environment = Environment.GetEnvironmentVariable("GSENVIRONMENT") ?? "DEV";
if (environment == "DEV")
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
}
else
{
    string connectionString = Environment.GetEnvironmentVariable("GSCONNECTIONSTRINGS");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString,
        assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
}

string[] allowedOrigins = JsonConvert.DeserializeObject<string[]>(Environment.GetEnvironmentVariable("GSALLOWEDORIGINSCORS"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());

    options.AddPolicy("AllowProduction",
        builder => builder.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader());

    options.AddPolicy("AllowStaging",
        builder => builder.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var key = Encoding.ASCII.GetBytes((new Key()).GetSecret());

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});



builder.Services.AddTransient<DataInsert>();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
    context.Database.Migrate();

    var dataInsert = serviceScope.ServiceProvider.GetRequiredService<DataInsert>();
    dataInsert.InitializeAsync().Wait();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string corsPolicy = environment switch
{
    "DEV" => "AllowLocalhost3000",
    "PROD" => "AllowProduction",
    "STAGING" => "AllowStaging",
    _ => throw new InvalidOperationException("Unknown environment")
};

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
