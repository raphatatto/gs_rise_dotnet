using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs;
using rise_gs.Models;
using rise_gs.Services;
using rise_gs.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly RiseContext _context;
        private readonly ILogger<UsuarioController> _logger;
        private readonly TokenService _tokenService;

        public UsuarioController(
            RiseContext context,
            ILogger<UsuarioController> logger,
            TokenService tokenService)
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }

        // GET api/v1/usuario?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetUsuarios(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("pageNumber e pageSize devem ser maiores que zero.");

            var query = _context.Usuarios.AsNoTracking();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var usuarios = await query
                .OrderBy(u => u.IdUsuario)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            string? BuildUrl(string actionName, object values)
                => Url?.Action(actionName, values);

            var items = usuarios.Select(u => new
            {
                u.IdUsuario,
                u.NomeUsuario,
                u.EmailUsuario,
                u.TipoUsuario,
                u.Telefone,
                descricao = u.Descricao,
                habilidades = u.Habilidades,
                links = new[]
                {
                    new { rel = "self", href = BuildUrl(nameof(GetUsuarioById), new { id = u.IdUsuario }), method = "GET" },
                    new { rel = "update", href = BuildUrl(nameof(UpdateUsuario), new { id = u.IdUsuario }), method = "PUT" },
                    new { rel = "delete", href = BuildUrl(nameof(DeleteUsuario), new { id = u.IdUsuario }), method = "DELETE" }
                }
            });

            var collectionLinks = new List<object>
            {
                new { rel = "self", href = BuildUrl(nameof(GetUsuarios), new { pageNumber, pageSize }), method = "GET" }
            };

            if (pageNumber < totalPages)
                collectionLinks.Add(new { rel = "next", href = BuildUrl(nameof(GetUsuarios), new { pageNumber = pageNumber + 1, pageSize }), method = "GET" });

            if (pageNumber > 1)
                collectionLinks.Add(new { rel = "prev", href = BuildUrl(nameof(GetUsuarios), new { pageNumber = pageNumber - 1, pageSize }), method = "GET" });

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalItems,
                totalPages,
                items,
                links = collectionLinks
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null) return NotFound();

            return Ok(new
            {
                usuario.IdUsuario,
                usuario.NomeUsuario,
                usuario.EmailUsuario,
                usuario.TipoUsuario,
                usuario.Telefone,
                descricao = usuario.Descricao,
                habilidades = usuario.Habilidades,
                links = new[]
                {
                    new { rel = "self", href = Url?.Action(nameof(GetUsuarioById), new { id }), method = "GET" },
                    new { rel = "update", href = Url?.Action(nameof(UpdateUsuario), new { id }), method = "PUT" },
                    new { rel = "delete", href = Url?.Action(nameof(DeleteUsuario), new { id }), method = "DELETE" }
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var model = new Usuario
            {
                NomeUsuario = dto.NomeUsuario,
                EmailUsuario = dto.EmailUsuario,
                SenhaUsuario = dto.SenhaUsuario,
                TipoUsuario = dto.TipoUsuario,
                Telefone = dto.Telefone,
                Descricao = dto.Descricao,
                Habilidades = dto.Habilidades,
            };

            _context.Usuarios.Add(model);
            await _context.SaveChangesAsync();

            var result = new
            {
                model.IdUsuario,
                model.NomeUsuario,
                model.EmailUsuario,
                model.TipoUsuario,
                model.Telefone,
                descricao = model.Descricao,
                habilidades = model.Habilidades
            };

            return CreatedAtAction(nameof(GetUsuarioById),
                new { id = model.IdUsuario },
                result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                    u.EmailUsuario == dto.EmailUsuario &&
                    u.SenhaUsuario == dto.SenhaUsuario);

            if (usuario == null)
            {
                return Unauthorized(new { message = "E-mail ou senha inválidos." });
            }

            var token = _tokenService.GenerateToken(usuario);

            var response = new LoginResponseDto
            {
                Token = token,
                ExpiraEm = DateTime.UtcNow.AddMinutes(60),
                IdUsuario = usuario.IdUsuario,
                NomeUsuario = usuario.NomeUsuario,
                TipoUsuario = usuario.TipoUsuario
            };

            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.NomeUsuario = dto.NomeUsuario;
            usuario.EmailUsuario = dto.EmailUsuario;
            usuario.TipoUsuario = dto.TipoUsuario;
            usuario.Telefone = dto.Telefone;
            usuario.Descricao = dto.Descricao;
            usuario.Habilidades = dto.Habilidades;

            if (!string.IsNullOrWhiteSpace(dto.SenhaUsuario))
                usuario.SenhaUsuario = dto.SenhaUsuario;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
