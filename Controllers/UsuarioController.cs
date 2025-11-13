using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.Models;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly RiseContext _context;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(RiseContext context, ILogger<UsuarioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/v1/usuario?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetUsuarios(
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("pageNumber e pageSize devem ser maiores que zero.");

            _logger.LogInformation("Listando usuários - página {pageNumber}, tamanho {pageSize}", pageNumber, pageSize);

            var query = _context.Usuarios.AsNoTracking();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var usuarios = await query
                .OrderBy(u => u.IdUsuario)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // HATEOAS: links da coleção
            var collectionLinks = new List<object>
            {
                new {
                    rel = "self",
                    href = Url.Action(nameof(GetUsuarios), values: new { pageNumber, pageSize }),
                    method = "GET"
                }
            };

            if (pageNumber < totalPages)
            {
                collectionLinks.Add(new
                {
                    rel = "next",
                    href = Url.Action(nameof(GetUsuarios), values: new { pageNumber = pageNumber + 1, pageSize }),
                    method = "GET"
                });
            }

            if (pageNumber > 1)
            {
                collectionLinks.Add(new
                {
                    rel = "prev",
                    href = Url.Action(nameof(GetUsuarios), values: new { pageNumber = pageNumber - 1, pageSize }),
                    method = "GET"
                });
            }

            var result = new
            {
                pageNumber,
                pageSize,
                totalItems,
                totalPages,
                items = usuarios.Select(u => new
                {
                    u.IdUsuario,
                    u.NomeUsuario,
                    u.EmailUsuario,
                    u.TipoUsuario,
                    links = new[]
                    {
                        new {
                            rel = "self",
                            href = Url.Action(nameof(GetUsuarioById), values: new { id = u.IdUsuario }),
                            method = "GET"
                        },
                        new {
                            rel = "delete",
                            href = Url.Action(nameof(DeleteUsuario), values: new { id = u.IdUsuario }),
                            method = "DELETE"
                        },
                        new {
                            rel = "update",
                            href = Url.Action(nameof(UpdateUsuario), values: new { id = u.IdUsuario }),
                            method = "PUT"
                        }
                    }
                }),
                links = collectionLinks
            };

            return Ok(result); // 200 OK
        }

        // GET api/v1/usuario/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(); // 404

            var result = new
            {
                usuario.IdUsuario,
                usuario.NomeUsuario,
                usuario.EmailUsuario,
                usuario.TipoUsuario,
                links = new[]
                {
                    new {
                        rel = "self",
                        href = Url.Action(nameof(GetUsuarioById), values: new { id }),
                        method = "GET"
                    },
                    new {
                        rel = "delete",
                        href = Url.Action(nameof(DeleteUsuario), values: new { id }),
                        method = "DELETE"
                    },
                    new {
                        rel = "update",
                        href = Url.Action(nameof(UpdateUsuario), values: new { id }),
                        method = "PUT"
                    }
                }
            };

            return Ok(result); // 200 OK
        }

        // POST api/v1/usuario
        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400

            _context.Usuarios.Add(model);
            await _context.SaveChangesAsync();

            var result = new
            {
                model.IdUsuario,
                model.NomeUsuario,
                model.EmailUsuario,
                model.TipoUsuario,
                links = new[]
                {
                    new {
                        rel = "self",
                        href = Url.Action(nameof(GetUsuarioById), values: new { id = model.IdUsuario }),
                        method = "GET"
                    },
                    new {
                        rel = "update",
                        href = Url.Action(nameof(UpdateUsuario), values: new { id = model.IdUsuario }),
                        method = "PUT"
                    },
                    new {
                        rel = "delete",
                        href = Url.Action(nameof(DeleteUsuario), values: new { id = model.IdUsuario }),
                        method = "DELETE"
                    }
                }
            };

            return CreatedAtAction(
                nameof(GetUsuarioById),
                new { id = model.IdUsuario },
                result
            ); // 201 Created
        }

        // PUT api/v1/usuario/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario model)
        {
            if (id != model.IdUsuario)
                return BadRequest("ID da URL é diferente do ID do corpo da requisição.");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(); // 404

            usuario.NomeUsuario = model.NomeUsuario;
            usuario.EmailUsuario = model.EmailUsuario;
            usuario.SenhaUsuario = model.SenhaUsuario;
            usuario.TipoUsuario = model.TipoUsuario;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // DELETE api/v1/usuario/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(); // 404

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
