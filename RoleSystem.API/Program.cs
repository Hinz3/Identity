using Common;
using EasyNetQDI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RoleSystem.Core;
using RoleSystem.Core.Configurations;
using RoleSystem.DataContext;
using Serilog;
using System.Security.Cryptography;
using RoleSystem.EventHandlers;
using RoleSystem.Contracts.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment)
        .Enrich.WithProperty("ApplicationLevel", hostingContext.HostingEnvironment.EnvironmentName);
});

builder.Services.AddHttpContextAccessor();
builder.Services.UseCommon();
builder.Services.UseContexts(builder.Configuration);
builder.Services.UseCore(builder.Configuration);
builder.Services.UseEventHandlers(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var jwtConfig = builder.Configuration.GetSection("JWT").Get<JWTConfiguration>();
builder.Services.AddSingleton<RsaSecurityKey>(provider =>
{
    // It's required to register the RSA key with depedency injection.
    // If you don't do this, the RSA instance will be prematurely disposed.

    RSA rsa = RSA.Create();
    rsa.ImportRSAPublicKey(
        source: Convert.FromBase64String(jwtConfig.PublicKey),
        bytesRead: out int _
    );

    return new RsaSecurityKey(rsa);
});

RSA rsa = RSA.Create();
rsa.ImportRSAPublicKey(
    source: Convert.FromBase64String(jwtConfig.PublicKey),
    bytesRead: out int _
);

var rsaKey = new RsaSecurityKey(rsa);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
#if DEBUG
    options.IncludeErrorDetails = true; // <- great for debugging
#endif
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = rsaKey,
        ValidAudience = jwtConfig.Audience,
        ValidIssuer = jwtConfig.Issuer,
        RequireSignedTokens = jwtConfig.RequireSignedTokens,
        RequireExpirationTime = jwtConfig.RequireExpirationTime, // <- JWTs are required to have "exp" property set
        ValidateLifetime = jwtConfig.ValidateLifetime, // <- the "exp" will be validated
        ValidateAudience = jwtConfig.ValidateAudience,
        ValidateIssuer = jwtConfig.ValidateIssuer,
    };
});

builder.Services.AddAuthorization(options =>
{
    var functions = Enum.GetValues(typeof(IdentityFunction)).Cast<IdentityFunction>().ToList();
    functions.ForEach(permission => options.AddPolicy(permission.ToString().Replace("_", "."), policy => policy.RequireClaim("UF", $"{(int)permission}")));
});

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Copy this into the value field: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

builder.Services.AddCors();

var app = builder.Build();
app.UseSubscriber();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }

    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
