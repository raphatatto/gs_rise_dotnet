using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs;
using rise_gs.Models;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TrilhaProgressoController : ControllerBase
    {
        private readonly RiseContext _context;
        private readonly ILogger<TrilhaProgressoController> _logger;

        public TrilhaProgressoController(RiseContext context, ILogger<TrilhaProgressoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/v1/trilhaprogresso?idUsuario=1
        [HttpGet]
        public async Task<IActionResult> GetTrilhas([FromQuery] int? idUsuario)
        {
            var query = _context.TrilhasProgresso.AsNoTracking();

            if (idUsuario.HasValue)
                query = query.Where(t => t.IdUsuario == idUsuario.Value);

            var result = await query
                .Select(t => new TrilhaProgressoDto
                {
                    IdTrilha = t.IdTrilha,
                    IdUsuario = t.IdUsuario,
                    PercentualConcluido = t.PercentualConcluido,
                    DtInicio = t.DtInicio,
                    DtUltimaAtualizacao = t.DtUltimaAtualizacao,
                    TituloTrilha = t.TituloTrilha,
                    CategoriaTrilha = t.CategoriaTrilha,
                    DataPlanejada = t.DataPlanejada,
                    DtCriacao = t.DtCriacao
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET api/v1/trilhaprogresso/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trilha = await _context.TrilhasProgresso
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdTrilha == id);

            if (trilha == null)
                return NotFound();

            var dto = new TrilhaProgressoDto
            {
                IdTrilha = trilha.IdTrilha,
                IdUsuario = trilha.IdUsuario,
                PercentualConcluido = trilha.PercentualConcluido,
                DtInicio = trilha.DtInicio,
                DtUltimaAtualizacao = trilha.DtUltimaAtualizacao,
                TituloTrilha = trilha.TituloTrilha,
                CategoriaTrilha = trilha.CategoriaTrilha,
                DataPlanejada = trilha.DataPlanejada,
                DtCriacao = trilha.DtCriacao
            };

            return Ok(dto);
        }

        // POST api/v1/trilhaprogresso
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrilhaProgressoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var trilha = new TrilhaProgresso
            {
                IdUsuario = dto.IdUsuario,
                TituloTrilha = dto.TituloTrilha,
                CategoriaTrilha = dto.CategoriaTrilha,
                DataPlanejada = dto.DataPlanejada,
                DtCriacao = DateTime.UtcNow
            };

            _context.TrilhasProgresso.Add(trilha);
            await _context.SaveChangesAsync();

            var result = new TrilhaProgressoDto
            {
                IdTrilha = trilha.IdTrilha,
                IdUsuario = trilha.IdUsuario,
                TituloTrilha = trilha.TituloTrilha,
                CategoriaTrilha = trilha.CategoriaTrilha,
                DataPlanejada = trilha.DataPlanejada,
                DtCriacao = trilha.DtCriacao
            };

            return CreatedAtAction(nameof(GetById), new { id = trilha.IdTrilha }, result);
        }

        // PUT api/v1/trilhaprogresso/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TrilhaProgressoUpdateDto dto)
        {
            var trilha = await _context.TrilhasProgresso.FirstOrDefaultAsync(t => t.IdTrilha == id);
            if (trilha == null)
                return NotFound();

            trilha.PercentualConcluido = dto.PercentualConcluido ?? trilha.PercentualConcluido;
            trilha.TituloTrilha = dto.TituloTrilha ?? trilha.TituloTrilha;
            trilha.CategoriaTrilha = dto.CategoriaTrilha ?? trilha.CategoriaTrilha;
            trilha.DataPlanejada = dto.DataPlanejada ?? trilha.DataPlanejada;
            trilha.DtUltimaAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/v1/trilhaprogresso/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trilha = await _context.TrilhasProgresso.FirstOrDefaultAsync(t => t.IdTrilha == id);
            if (trilha == null)
                return NotFound();

            _context.TrilhasProgresso.Remove(trilha);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
