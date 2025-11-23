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

        [HttpGet]
        public async Task<IActionResult> GetCurriculos([FromQuery] int? idUsuario)
        {
            var query = _context.Curriculos.AsNoTracking();

            if (idUsuario.HasValue)
                query = query.Where(c => c.IdUsuario == idUsuario.Value);

            var curriculos = await query
                .OrderByDescending(c => c.UltimaAtualizacao)
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

            if (curriculo == null) return NotFound();
            return Ok(curriculo);
        }

        // POST api/v1/curriculo  (UPsert por IdUsuario)
        [HttpPost]
        public async Task<IActionResult> UpsertCurriculo([FromBody] CurriculoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdUsuario <= 0)
                return BadRequest(new { error = "IdUsuario é obrigatório." });

            if (string.IsNullOrWhiteSpace(dto.Habilidades))
                return BadRequest(new { error = "Habilidades é obrigatório." });

            var existing = await _context.Curriculos
                .FirstOrDefaultAsync(c => c.IdUsuario == dto.IdUsuario);

            // Não existe → cria
            if (existing == null)
            {
                var entity = new Curriculo
                {
                    TituloCurriculo = dto.TituloCurriculo,
                    ExperienciaProfissional = dto.ExperienciaProfissional,
                    Habilidades = dto.Habilidades,
                    Formacao = dto.Formacao,
                    UltimaAtualizacao = dto.UltimaAtualizacao ?? DateTime.UtcNow,
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

            // Já existe → atualiza
            existing.TituloCurriculo = dto.TituloCurriculo ?? existing.TituloCurriculo;
            existing.ExperienciaProfissional = dto.ExperienciaProfissional ?? existing.ExperienciaProfissional;
            existing.Habilidades = dto.Habilidades ?? existing.Habilidades;
            existing.Formacao = dto.Formacao ?? existing.Formacao;
            existing.Projetos = dto.Projetos ?? existing.Projetos;
            existing.Links = dto.Links ?? existing.Links;
            existing.UltimaAtualizacao = dto.UltimaAtualizacao ?? DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updated = new CurriculoDto
            {
                IdCurriculo = existing.IdCurriculo,
                TituloCurriculo = existing.TituloCurriculo,
                ExperienciaProfissional = existing.ExperienciaProfissional,
                Habilidades = existing.Habilidades,
                Formacao = existing.Formacao,
                UltimaAtualizacao = existing.UltimaAtualizacao,
                Projetos = existing.Projetos,
                Links = existing.Links,
                IdUsuario = existing.IdUsuario
            };

            return Ok(updated);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCurriculo(int id, [FromBody] CurriculoUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(c => c.IdCurriculo == id);
            if (curriculo == null) return NotFound();

            // Apenas atualiza se veio no DTO
            curriculo.TituloCurriculo = dto.TituloCurriculo ?? curriculo.TituloCurriculo;
            curriculo.ExperienciaProfissional = dto.ExperienciaProfissional ?? curriculo.ExperienciaProfissional;
            curriculo.Habilidades = dto.Habilidades ?? curriculo.Habilidades;
            curriculo.Formacao = dto.Formacao ?? curriculo.Formacao;
            curriculo.Projetos = dto.Projetos ?? curriculo.Projetos;
            curriculo.Links = dto.Links ?? curriculo.Links;
            curriculo.UltimaAtualizacao = dto.UltimaAtualizacao ?? DateTime.UtcNow;

            if (dto.IdUsuario.HasValue)
                curriculo.IdUsuario = dto.IdUsuario.Value;

            await _context.SaveChangesAsync();
            return NoContent();
        }

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
