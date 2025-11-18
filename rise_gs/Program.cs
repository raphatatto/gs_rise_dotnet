using Microsoft.EntityFrameworkCore;
using rise_gs;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// DbContext + Oracle
builder.Services.AddDbContext<RiseContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HealthChecks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Middleware de "tracing" simples: gera um TraceId por request
app.Use(async (context, next) =>
{
    var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
    context.Items["TraceId"] = traceId;
    context.Response.Headers["X-Trace-Id"] = traceId;

    app.Logger.LogInformation("Request {TraceId} {Method} {Path}",
        traceId,
        context.Request.Method,
        context.Request.Path);

    await next();
});

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Health endpoint
app.MapHealthChecks("/health");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
