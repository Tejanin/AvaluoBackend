using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Middlewares;
using AvaluoAPI.Domain.Services.MetodoEvaluacionService;
using AvaluoAPI.Domain.Services.TipoInformeService;
using AvaluoAPI.Domain.Services.TipoCompetenciaService;
using AvaluoAPI.Domain.Services.UsuariosService;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Middlewares;
using AvaluoAPI.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Utilities.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Validaci�n auto de ModelState desactivada
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterDTO>(); // Filtro para que las validaciones de los DTO traiga el misnmo formato que el middleware
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Forzar PascalCase en las respuestas JSON.
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                }
            },
            new string[]{ }
        }
    });
});
builder.Services.AddDbContext<AvaluoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton<DapperContext>();

// UoW
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AvaluoMapper
builder.Services.AddMappingConfiguration();

// Services
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


builder.Services.AddScoped<IMetodoEvaluacionService, MetodoEvaluacionService>();

builder.Services.AddScoped<ITipoInformeService, TipoInformeService>();

builder.Services.AddScoped<ITipoCompetenciaService, TipoCompetenciaService>();

builder.Services.AddScoped<ICompetenciaService, CompetenciaService>();

// FileHandler

builder.Services.AddSingleton<FileHandler>();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
}
);
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "X-Permissions";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddScoped<IClaimsFactory, ClaimsFactory>();
// Authorization
builder.Services.AddAuthorization();


// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    builder.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());


});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AvaluoDbContext>();
    context.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
//app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
