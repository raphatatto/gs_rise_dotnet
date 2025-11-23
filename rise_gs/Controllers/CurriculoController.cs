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

<<<<<<< HEAD
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

=======
            var curriculos = await query.OrderByDescending(c => c.UltimaAtualizacao).ToListAsync();
>>>>>>> bd27691 (adicionando IA)
            return Ok(curriculos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurriculoById(int id)
        {
<<<<<<< HEAD
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
=======
            var curriculo = await _context.Curriculos.AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCurriculo == id);
>>>>>>> bd27691 (adicionando IA)

            if (curriculo == null) return NotFound();
            return Ok(curriculo);
        }

        [HttpPost]
<<<<<<< HEAD
        public async Task<IActionResult> CreateCurriculo([FromBody] CurriculoCreateDto dto)
=======
        public async Task<IActionResult> UpsertCurriculo([FromBody] Curriculo model)
>>>>>>> bd27691 (adicionando IA)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (model.IdUsuario <= 0)
                return BadRequest(new { error = "IdUsuario é obrigatório." });

            if (string.IsNullOrWhiteSpace(model.Habilidades))
                return BadRequest(new { error = "Habilidades é obrigatório." });

            var existing = await _context.Curriculos
                .FirstOrDefaultAsync(c => c.IdUsuario == model.IdUsuario);

            if (existing == null)
            {
                model.UltimaAtualizacao = model.UltimaAtualizacao ?? DateTime.UtcNow;
                _context.Curriculos.Add(model);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCurriculoById), new { id = model.IdCurriculo }, model);
            }

            existing.TituloCurriculo = model.TituloCurriculo;
            existing.ExperienciaProfissional = model.ExperienciaProfissional;
            existing.Habilidades = model.Habilidades;
            existing.Formacao = model.Formacao;
            existing.Projetos = model.Projetos;
            existing.Links = model.Links;
            existing.UltimaAtualizacao = model.UltimaAtualizacao ?? DateTime.UtcNow;

<<<<<<< HEAD
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
=======
            await _context.SaveChangesAsync();
            return Ok(existing);
>>>>>>> bd27691 (adicionando IA)
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCurriculo(int id, [FromBody] CurriculoUpdateDto dto)
        {
<<<<<<< HEAD
            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(c => c.IdCurriculo == id);
=======
            if (!ModelState.IsValid) return BadRequest(ModelState);
>>>>>>> bd27691 (adicionando IA)

            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(c => c.IdCurriculo == id);
            if (curriculo == null) return NotFound();

<<<<<<< HEAD
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
=======
            curriculo.TituloCurriculo = model.TituloCurriculo;
            curriculo.ExperienciaProfissional = model.ExperienciaProfissional;
            curriculo.Habilidades = model.Habilidades;
            curriculo.Formacao = model.Formacao;
            curriculo.UltimaAtualizacao = model.UltimaAtualizacao ?? DateTime.UtcNow;
            curriculo.Projetos = model.Projetos;
            curriculo.Links = model.Links;
            curriculo.IdUsuario = model.IdUsuario;
>>>>>>> bd27691 (adicionando IA)

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCurriculo(int id)
        {
            var curriculo = await _context.Curriculos.FirstOrDefaultAsync(c => c.IdCurriculo == id);
<<<<<<< HEAD

            if (curriculo == null)
                return NotFound();
=======
            if (curriculo == null) return NotFound();
>>>>>>> bd27691 (adicionando IA)

            _context.Curriculos.Remove(curriculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
