using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rise_gs;
using rise_gs.Services;
<<<<<<< HEAD

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<RiseContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Configura seção Jwt para o TokenService
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// 🔹 PEGA AS CONFIGS BRUTAS
var config = builder.Configuration;

// 🔹 Lê a Key diretamente (e garante fallback)
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
        options.RequireHttpsMetadata = false; // em prod: true
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

// Controllers / Swagger / etc.
=======
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RiseContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

>>>>>>> bd27691 (adicionando IA)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient<IAiCurriculoService, AiCurriculoService>();

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

<<<<<<< HEAD
// tracing simples
=======
app.UseRouting();
app.UseCors("DevCors");

>>>>>>> bd27691 (adicionando IA)
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

<<<<<<< HEAD
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

=======
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
>>>>>>> bd27691 (adicionando IA)
app.Run();
