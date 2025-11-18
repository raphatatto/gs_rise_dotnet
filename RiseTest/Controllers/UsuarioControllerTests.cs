using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rise_gs.Controllers;
using rise_gs.DTOs;
using rise_gs.DTOs;
using rise_gs.Models;
using RiseTest.Helpers;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
namespace RiseTest.Controllers
{
    public class UsuarioControllerTests
    {
        private UsuarioController CreateControllerWithData()
        {
            var context = TestDbContextFactory.CreateInMemoryContext();

            context.Usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                NomeUsuario = "raphaela",
                EmailUsuario = "rapha@example.com",
                SenhaUsuario = "123456",
                TipoUsuario = "cliente"
            });

            context.Usuarios.Add(new Usuario
            {
                IdUsuario = 2,
                NomeUsuario = "admin",
                EmailUsuario = "admin@example.com",
                SenhaUsuario = "admin123",
                TipoUsuario = "admin"
            });

            context.SaveChanges();

            var logger = LoggerFactory.Create(b => b.AddDebug()).CreateLogger<UsuarioController>();

            return new UsuarioController(context, logger);
        }

        [Fact]
        public async Task GetUsuarios_DeveRetornarOkComPaginacao()
        {
            // Arrange
            var controller = CreateControllerWithData();

            // Act
            var result = await controller.GetUsuarios(pageNumber: 1, pageSize: 10);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            // Serializa o objeto retornado para JSON
            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // totalItems do JSON
            var totalItems = root.GetProperty("totalItems").GetInt32();
            totalItems.Should().Be(2);

            // opcional: garantir que vieram 2 items na coleção
            var items = root.GetProperty("items");
            items.GetArrayLength().Should().Be(2);
        }

        [Fact]
        public async Task Login_ComCredenciaisValidas_DeveRetornarOk()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryContext();

            context.Usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                NomeUsuario = "raphaela",
                EmailUsuario = "rapha@example.com",
                SenhaUsuario = "123456",
                TipoUsuario = "cliente"
            });
            context.SaveChanges();

            var logger = LoggerFactory.Create(b => b.AddDebug()).CreateLogger<UsuarioController>();
            var controller = new UsuarioController(context, logger);

            var dto = new LoginRequestDto
            {
                NomeUsuario = "raphaela",
                SenhaUsuario = "123456"
            };

            // Act
            var result = await controller.Login(dto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            // Serializa pra JSON e verifica o campo "nomeUsuario"
            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var nomeUsuario = root.GetProperty("NomeUsuario").GetString();

            nomeUsuario.Should().Be("raphaela");
        }

        [Fact]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornarUnauthorized()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryContext();

            context.Usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                NomeUsuario = "raphaela",
                EmailUsuario = "rapha@example.com",
                SenhaUsuario = "123456",
                TipoUsuario = "cliente"
            });
            context.SaveChanges();

            var logger = LoggerFactory.Create(b => b.AddDebug()).CreateLogger<UsuarioController>();
            var controller = new UsuarioController(context, logger);

            var dto = new LoginRequestDto
            {
                NomeUsuario = "raphaela",
                SenhaUsuario = "errada"
            };

            // Act
            var result = await controller.Login(dto);

            // Assert
            var unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Should().NotBeNull();
            unauthorized!.StatusCode.Should().Be(401);
        }
    }
}
