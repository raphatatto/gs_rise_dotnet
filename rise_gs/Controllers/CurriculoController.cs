using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.Models;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CurriculoController : ControllerBase
    {
        private readonly RiseContext _context;
        private readonly ILogger<CurriculoController> _logger;

        public CurriculoController(RiseContext context, ILogger<CurriculoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/v1/curriculo?idUsuario=1
        [HttpGet]
        public async Task<IActionResult> GetCurriculos([FromQuery] int? idUsuario)
        {
            _logger.LogInformation("Listando currículos. idUsuario = {idUsuario}", idUsuario);

            var query = _context.Curriculos.AsNoTracking();

            if (idUsuario.HasValue)
                query = query.Where(c => c.IdUsuario == idUsuario.Value);

            var curriculos = await query.ToListAsync();
            return Ok(curriculos);
        }

        // GET api/v1/curriculo/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurriculoById(int id)
        {
            var curriculo = await _context.Curriculos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCurriculo == id);

            if (curriculo == null)
                return NotFound();

            return Ok(curriculo);
        }

        // POST api/v1/curriculo
        [HttpPost]
        public async Task<IActionResult> CreateCurriculo([FromBody] Curriculo model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Curriculos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurriculoById),
                new { id = model.IdCurriculo },
                model);
        }

        // PUT api/v1/curriculo/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCurriculo(int id, [FromBody] Curriculo model)
        {
            var curriculo = await _context.Curriculos
                .FirstOrDefaultAsync(c => c.IdCurriculo == id);

            if (curriculo == null)
                return NotFound();

            curriculo.TituloCurriculo = model.TituloCurriculo;
            curriculo.ExperienciaProfissional = model.ExperienciaProfissional;
            curriculo.Habilidades = model.Habilidades;
            curriculo.Formacao = model.Formacao;
            curriculo.UltimaAtualizacao = model.UltimaAtualizacao;
            curriculo.Projetos = model.Projetos;
            curriculo.Links = model.Links;
            curriculo.IdUsuario = model.IdUsuario;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/v1/curriculo/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCurriculo(int id)
        {
            var curriculo = await _context.Curriculos
                .FirstOrDefaultAsync(c => c.IdCurriculo == id);

            if (curriculo == null)
                return NotFound();

            _context.Curriculos.Remove(curriculo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
