using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return Ok(registros);
        }

        // GET api/v1/bemestar/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var registro = await _context.BemEstares.FindAsync(id);
            if (registro == null)
                return NotFound();

            return Ok(registro);
        }

        // POST api/v1/bemestar
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BemEstar model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.BemEstares.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = model.IdBemEstar },
                model);
        }

        // PUT api/v1/bemestar/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BemEstar model)
        {
            if (id != model.IdBemEstar)
                return BadRequest("ID da URL difere do corpo.");

            var registro = await _context.BemEstares.FindAsync(id);
            if (registro == null)
                return NotFound();

            registro.DtRegistro = model.DtRegistro;
            registro.NivelHumor = model.NivelHumor;
            registro.HorasEstudo = model.HorasEstudo;
            registro.DescAtividade = model.DescAtividade;
            registro.IdUsuario = model.IdUsuario;

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
