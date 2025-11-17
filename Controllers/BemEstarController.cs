using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs;
using rise_gs.Models;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BemEstarController : ControllerBase
    {
        private readonly RiseContext _context;
        private readonly ILogger<BemEstarController> _logger;

        public BemEstarController(RiseContext context, ILogger<BemEstarController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/v1/bemestar?idUsuario=1
        [HttpGet]
        public async Task<IActionResult> GetRegistros([FromQuery] int? idUsuario)
        {
            _logger.LogInformation("Listando bem-estar. idUsuario = {idUsuario}", idUsuario);

            var query = _context.BemEstares.AsNoTracking();

            if (idUsuario.HasValue)
                query = query.Where(b => b.IdUsuario == idUsuario.Value);

            var registros = await query.ToListAsync();

            var result = registros.Select(b => new BemEstarDto
            {
                IdBemEstar = b.IdBemEstar,
                DtRegistro = b.DtRegistro,
                NivelHumor = b.NivelHumor,
                HorasEstudo = b.HorasEstudo,
                DescAtividade = b.DescAtividade,
                IdUsuario = b.IdUsuario
            });

            return Ok(result);
        }

        // GET api/v1/bemestar/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var registro = await _context.BemEstares.FindAsync(id);
            if (registro == null)
                return NotFound();

            var result = new BemEstarDto
            {
                IdBemEstar = registro.IdBemEstar,
                DtRegistro = registro.DtRegistro,
                NivelHumor = registro.NivelHumor,
                HorasEstudo = registro.HorasEstudo,
                DescAtividade = registro.DescAtividade,
                IdUsuario = registro.IdUsuario
            };

            return Ok(result);
        }

        // POST api/v1/bemestar
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BemEstarCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new BemEstar
            {
                DtRegistro = dto.DtRegistro,
                NivelHumor = dto.NivelHumor,
                HorasEstudo = dto.HorasEstudo,
                DescAtividade = dto.DescAtividade,
                IdUsuario = dto.IdUsuario
            };

            _context.BemEstares.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = entity.IdBemEstar },
                entity);
        }

        // PUT api/v1/bemestar/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BemEstarUpdateDto dto)
        {
            var registro = await _context.BemEstares.FindAsync(id);
            if (registro == null)
                return NotFound();

            registro.DtRegistro = dto.DtRegistro;
            registro.NivelHumor = dto.NivelHumor;
            registro.HorasEstudo = dto.HorasEstudo;
            registro.DescAtividade = dto.DescAtividade;
            registro.IdUsuario = dto.IdUsuario;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/v1/bemestar/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _context.BemEstares.FindAsync(id);
            if (registro == null)
                return NotFound();

            _context.BemEstares.Remove(registro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
