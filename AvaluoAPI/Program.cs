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
using System.Text.Json.Serialization;
using AvaluoAPI.Domain.Services.EdificioService;
using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Utilities.JWT;
using AvaluoAPI.Domain.Services.EstadoService;
using Swashbuckle.AspNetCore.Filters;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Domain.Services.RubricasService;
using Quartz;
using AvaluoAPI.Infrastructure.Jobs.Configuration;
using AvaluoAPI.Domain.Services.AsignaturaService;
using AvaluoAPI.Domain.Services.AulaService;
using AvaluoAPI.Domain.Services.AreaService;
using AvaluoAPI.Domain.Services.CarreraService;
using AvaluoAPI.Domain.Services.DesempeñoService;
using StackExchange.Redis;
using AvaluoAPI.Domain.Services.RolService;
using AvaluoAPI.Domain.Services.ContactoService;
using AvaluoAPI.Domain.Services.DashboardService;
using AvaluoAPI.Infrastructure.Persistence.Repositories.ContactoRepositories;
using AvaluoAPI.Infrastructure.Persistence.Repositories.RolRepositories;
using AvaluoAPI.Domain.Services.InventarioService;
using AvaluoAPI.Infrastructure.Persistence.Repositories.InventarioRepositories;
using AvaluoAPI.Domain.Services.PIService;
using AvaluoAPI.Domain.Services.InformeService;
using AvaluoAPI.Domain.Services.HistorialIncumplimientoService;


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
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Avaluo",
        Version = "v1"
    });

    
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

builder.WebHost.UseUrls( "https://localhost:8000");

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
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


builder.Services.AddScoped<IEdificioService,  EdificioService>();

builder.Services.AddScoped<IPiService, PiService>();
builder.Services.AddScoped<ICompetenciaService, CompetenciaService>();

builder.Services.AddScoped<IEstadoService, EstadoService>();

builder.Services.AddScoped<IRubricaService, RubricaService>();

builder.Services.AddScoped<IintecService,INTECServiceMock>();

builder.Services.AddScoped<IAsignaturaService, AsignaturaService>();
builder.Services.AddScoped<IAulaService, AulaService>();
builder.Services.AddScoped<IAreaService, AreaService>();

builder.Services.AddScoped<ICarreraService, CarreraService>();
builder.Services.AddScoped<IDesempeñoService, DesempeñoService>();

builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IContactoRepository, ContactoRepository>();

builder.Services.AddScoped<IInventarioService, InventarioService>();

builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();

builder.Services.AddScoped<IInformeService, InformeService>();

builder.Services.AddScoped<IHistorialIncumplimientoService, HistorialIncumplimientoService>();



// FileHandler

builder.Services.AddSingleton<FileHandler>();

// EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// Jobs
//builder.Services.ConfigureQuartz();

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ResumenInstance_";
});
builder.Services.AddScoped<IResumenRedisService, ResumenRedisService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connectionString!);
});
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
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "X-Permissions";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Services.AddSingleton<IClaimsFactory, ClaimsFactory>();
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

builder.Services.AddControllersWithViews(); //Para poder ver las views de los informes en las rutas

builder.Services.AddScoped<PdfHelper>(); // Registra PdfHelper como un servicio

// Llave para poder usar ironPdf
IronPdf.License.LicenseKey = builder.Configuration["IronPdf:LicenseKey"];

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Esto hace que Swagger UI sea la página raíz
    });
}

// Middlewares
//app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthMiddleware();
// En Program.cs
app.UseStaticFiles(); // Esto habilita servir archivos desde wwwroot

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
