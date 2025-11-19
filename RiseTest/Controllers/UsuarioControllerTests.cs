using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rise_gs.Controllers;
using rise_gs.DTOs.Usuario;      // 👈 DTOs de usuário (LoginRequestDto, UsuarioCreateDto etc.)
using rise_gs.Models;
using rise_gs.Services;          // 👈 JwtSettings e TokenService
using RiseTest.Helpers;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RiseTest.Controllers
{
    public class UsuarioControllerTests
    {
        /// <summary>
        /// Cria um UsuarioController com:
        /// - DbContext em memória preenchido
        /// - Logger
        /// - TokenService com JwtSettings de teste
        /// </summary>
        private UsuarioController CreateControllerWithData()
        {
            var context = TestDbContextFactory.CreateInMemoryContext();

            // Seed de usuários de teste
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

            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
            var logger = loggerFactory.CreateLogger<UsuarioController>();

            // JwtSettings de TESTE (não usar em produção)
            var jwtSettings = new JwtSettings
            {
                Key = "ChaveDeTesteSuperSegura_1234567890_TESTE", // >= 32 chars
                Issuer = "rise-api-tests",
                Audience = "rise-api-tests",
                ExpireMinutes = 60
            };

            var tokenService = new TokenService(Options.Create(jwtSettings));

            return new UsuarioController(context, logger, tokenService);
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

            // garantir que vieram 2 items na coleção
            var items = root.GetProperty("items");
            items.GetArrayLength().Should().Be(2);
        }

        [Fact]
        public async Task Login_ComCredenciaisValidas_DeveRetornarOk()
        {
            // Arrange
            var controller = CreateControllerWithData();

            var dto = new LoginRequestDto
            {
                EmailUsuario = "rapha@example.com",
                SenhaUsuario = "123456"
            };

            // Act
            var result = await controller.Login(dto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var json = JsonSerializer.Serialize(okResult.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // 👇 AGORA usando PascalCase, igual ao DTO
            var nomeUsuario = root.GetProperty("NomeUsuario").GetString();
            nomeUsuario.Should().Be("raphaela");

            var token = root.GetProperty("Token").GetString();
            token.Should().NotBeNullOrEmpty();
        }


        [Fact]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornarUnauthorized()
        {
            // Arrange
            var controller = CreateControllerWithData();

            var dto = new LoginRequestDto
            {
                EmailUsuario = "rapha@example.com",
                SenhaUsuario = "senhaErrada"
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
