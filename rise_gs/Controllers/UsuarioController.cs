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
        private readonly TokenService _tokenService;   // 👈 ADICIONAR ISSO

        public UsuarioController(
            RiseContext context,
            ILogger<UsuarioController> logger,
            TokenService tokenService)                  // já está certo aqui
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;              // 👈 E ISSO
        }

<<<<<<< HEAD
        // GET api/v1/usuario?pageNumber=1&pageSize=10
=======
>>>>>>> bd27691 (adicionando IA)
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

<<<<<<< HEAD
=======
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

>>>>>>> bd27691 (adicionando IA)
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
<<<<<<< HEAD
                Habilidades = dto.Habilidades,
=======
                Habilidades = dto.Habilidades
>>>>>>> bd27691 (adicionando IA)
            };

            _context.Usuarios.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarioById), new { id = model.IdUsuario }, new
            {
                model.IdUsuario,
                model.NomeUsuario,
                model.EmailUsuario,
                model.TipoUsuario,
                model.Telefone,
<<<<<<< HEAD
                model.Descricao,
                model.Habilidades,
            };

            return CreatedAtAction(nameof(GetUsuarioById),
                new { id = model.IdUsuario },
                result);
=======
                descricao = model.Descricao,
                habilidades = model.Habilidades
            });
>>>>>>> bd27691 (adicionando IA)
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
<<<<<<< HEAD
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
=======
                return Unauthorized("E-mail ou senha inválidos.");

            return Ok(new
            {
                idUsuario = usuario.IdUsuario,
                nomeUsuario = usuario.NomeUsuario,
                emailUsuario = usuario.EmailUsuario,
                tipoUsuario = usuario.TipoUsuario,
                telefone = usuario.Telefone,
                descricao = usuario.Descricao,
                habilidades = usuario.Habilidades
            });
>>>>>>> bd27691 (adicionando IA)
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.NomeUsuario = dto.NomeUsuario;
            usuario.EmailUsuario = dto.EmailUsuario;
<<<<<<< HEAD
            usuario.Telefone = dto.Telefone;
            usuario.Descricao = dto.Descricao;
            usuario.Habilidades = dto.Habilidades;
            
=======
            usuario.TipoUsuario = dto.TipoUsuario;
            usuario.Telefone = dto.Telefone;
            usuario.Descricao = dto.Descricao;
            usuario.Habilidades = dto.Habilidades;
>>>>>>> bd27691 (adicionando IA)

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