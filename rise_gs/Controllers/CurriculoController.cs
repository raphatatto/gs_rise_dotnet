using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs.Curriculo;
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

            var curriculos = await query
                .Select(c => new CurriculoDto
                {
                    IdCurriculo = c.IdCurriculo,
                    TituloCurriculo = c.TituloCurriculo,
                    ExperienciaProfissional = c.ExperienciaProfissional,
                    Habilidades = c.Habilidades,
                    Formacao = c.Formacao,
                    UltimaAtualizacao = c.UltimaAtualizacao,
                    Projetos = c.Projetos,
                    Links = c.Links,
                    IdUsuario = c.IdUsuario
                })
                .ToListAsync();

            return Ok(curriculos);
        }

        // GET api/v1/curriculo/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurriculoById(int id)
        {
            var curriculo = await _context.Curriculos
                .AsNoTracking()
                .Where(c => c.IdCurriculo == id)
                .Select(c => new CurriculoDto
                {
                    IdCurriculo = c.IdCurriculo,
                    TituloCurriculo = c.TituloCurriculo,
                    ExperienciaProfissional = c.ExperienciaProfissional,
                    Habilidades = c.Habilidades,
                    Formacao = c.Formacao,
                    UltimaAtualizacao = c.UltimaAtualizacao,
                    Projetos = c.Projetos,
                    Links = c.Links,
                    IdUsuario = c.IdUsuario
                })
                .FirstOrDefaultAsync();

            if (curriculo == null)
                return NotFound();

            return Ok(curriculo);
        }

        // POST api/v1/curriculo
        [HttpPost]
        public async Task<IActionResult> CreateCurriculo([FromBody] CurriculoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new Curriculo
            {
                TituloCurriculo = dto.TituloCurriculo,
                ExperienciaProfissional = dto.ExperienciaProfissional,
                Habilidades = dto.Habilidades,
                Formacao = dto.Formacao,
                UltimaAtualizacao = dto.UltimaAtualizacao,
                Projetos = dto.Projetos,
                Links = dto.Links,
                IdUsuario = dto.IdUsuario
            };

            _context.Curriculos.Add(entity);
            await _context.SaveChangesAsync();

            var response = new CurriculoDto
            {
                IdCurriculo = entity.IdCurriculo,
                TituloCurriculo = entity.TituloCurriculo,
                ExperienciaProfissional = entity.ExperienciaProfissional,
                Habilidades = entity.Habilidades,
                Formacao = entity.Formacao,
                UltimaAtualizacao = entity.UltimaAtualizacao,
                Projetos = entity.Projetos,
                Links = entity.Links,
                IdUsuario = entity.IdUsuario
            };

            return CreatedAtAction(nameof(GetCurriculoById),
                new { id = entity.IdCurriculo },
                response);
        }

        // PUT api/v1/curriculo/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCurriculo(int id, [FromBody] CurriculoUpdateDto dto)
        {
            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(c => c.IdCurriculo == id);

            if (curriculo == null)
                return NotFound();

            // Apenas atualiza se veio no DTO
            curriculo.TituloCurriculo = dto.TituloCurriculo ?? curriculo.TituloCurriculo;
            curriculo.ExperienciaProfissional = dto.ExperienciaProfissional ?? curriculo.ExperienciaProfissional;
            curriculo.Habilidades = dto.Habilidades ?? curriculo.Habilidades;
            curriculo.Formacao = dto.Formacao ?? curriculo.Formacao;
            curriculo.UltimaAtualizacao = dto.UltimaAtualizacao ?? curriculo.UltimaAtualizacao;
            curriculo.Projetos = dto.Projetos ?? curriculo.Projetos;
            curriculo.Links = dto.Links ?? curriculo.Links;

            if (dto.IdUsuario.HasValue)
                curriculo.IdUsuario = dto.IdUsuario.Value;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/v1/curriculo/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCurriculo(int id)
        {
            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(c => c.IdCurriculo == id);

            if (curriculo == null)
                return NotFound();

            _context.Curriculos.Remove(curriculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
