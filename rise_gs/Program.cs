using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rise_gs;
using rise_gs.Services;

var builder = WebApplication.CreateBuilder(args);

// ===================== DB CONTEXT =====================
builder.Services.AddDbContext<RiseContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// ===================== JWT CONFIG =====================
// Configura seção Jwt para o TokenService
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Pega config bruta
var config = builder.Configuration;

// Key
var jwtKey = config["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    Console.WriteLine("⚠ Jwt:Key veio vazia. Usando chave padrão apenas para DESENVOLVIMENTO.");
    jwtKey = "SenhaSegura_super_grande_123!";
}

var jwtIssuer = config["Jwt:Issuer"] ?? "rise-api";
var jwtAudience = config["Jwt:Audience"] ?? "rise-clients";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
var signingKey = new SymmetricSecurityKey(keyBytes);

// TokenService
builder.Services.AddScoped<TokenService>();

// Autenticação JWT
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // em produção: true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = signingKey
        };
    });

// ===================== SERVICES GERAIS =====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// IA Currículo
builder.Services.AddHttpClient<IAiCurriculoService, AiCurriculoService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ===================== MIDDLEWARE =====================

// tracing simples
app.Use(async (context, next) =>
{
    var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
    context.Response.Headers["X-Trace-Id"] = traceId;
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("DevCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
