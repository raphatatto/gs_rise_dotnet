using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiseTest.Helpers;
using rise_gs.Controllers;
using rise_gs.DTOs;
using rise_gs.Models;
using Xunit;

namespace RiseTest.Controllers
{
    public class BemEstarControllerTests
    {
        [Fact]
        public async Task Create_DeveCriarRegistroERetornarCreated()
        {
            var context = TestDbContextFactory.CreateInMemoryContext();

            context.Usuarios.Add(new Usuario
            {
                IdUsuario = 1,
                NomeUsuario = "rapha",
                EmailUsuario = "rapha@example.com",
                SenhaUsuario = "123",
                TipoUsuario = "cliente"
            });
            context.SaveChanges();

            var logger = LoggerFactory.Create(b => b.AddDebug()).CreateLogger<BemEstarController>();
            var controller = new BemEstarController(context, logger);

            // Substitua a criação do objeto BemEstar por BemEstarCreateDto
            var model = new BemEstarCreateDto
            {
                DtRegistro = DateTime.UtcNow,
                NivelHumor = 8,
                HorasEstudo = TimeSpan.FromHours(2),
                DescAtividade = "Estudei .NET",
                IdUsuario = 1
            };

            // Act
            var result = await controller.Create(model);

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.StatusCode.Should().Be(201);

            context.BemEstares.Should().HaveCount(1);
        }
    }
}
