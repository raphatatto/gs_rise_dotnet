using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rise_gs;
using rise_gs.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 DbContext (Oracle)
builder.Services.AddDbContext<RiseContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// 🔹 Configura o JwtSettings a partir do appsettings.json
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

// 🔹 Faz o bind fortemente tipado e garante defaults úteis
var jwtSettings = jwtSection.Get<JwtSettings>() ?? new JwtSettings();

if (string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    Console.WriteLine("⚠️  Jwt:Key não foi configurado. Aplicando valor padrão apenas para desenvolvimento.");
    jwtSettings.Key = "SenhaSegura_super_grande_123!";
}

if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
{
    jwtSettings.Issuer = "rise-api";
}

if (string.IsNullOrWhiteSpace(jwtSettings.Audience))
{
    jwtSettings.Audience = "rise-clients";
}

if (jwtSettings.ExpireMinutes <= 0)
{
    jwtSettings.ExpireMinutes = 60;
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

// 🔹 Registra o TokenService na DI
builder.Services.AddScoped<TokenService>();

// 🔹 Configura autenticação JWT
builder.Services.AddAuthentication(options =>
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
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = signingKey
    };
});

// 🔹 Controllers / Swagger / HealthChecks
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Middleware de tracing
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

app.UseAuthentication();  // 👈 JWT entra aqui
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
