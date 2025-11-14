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

        // GET api/v1/trilhaprogresso/usuario/5
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<IActionResult> GetByUsuario(int idUsuario)
        {
            var trilha = await _context.TrilhasProgresso
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdUsuario == idUsuario);

            if (trilha == null)
                return NotFound();

            var result = new TrilhaUpdateDto
            {
                IdUsuario = trilha.IdUsuario,
                PercentualConcluido = trilha.PercentualConcluido,
                DtInicio = trilha.DtInicio,
                DtUltimaAtualizacao = trilha.DtUltimaAtualizacao
            };


            return Ok(trilha);
        }

        // POST api/v1/trilhaprogresso
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrilhaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var trilha = new TrilhaProgresso
            {
                IdUsuario = dto.IdUsuario,
                PercentualConcluido = dto.PercentualConcluido,
                DtInicio = dto.DtInicio,
                DtUltimaAtualizacao = DateTime.UtcNow
            };

            _context.TrilhasProgresso.Add(trilha);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByUsuario),
                new { idUsuario = trilha.IdUsuario },
                trilha);
        }

        // PUT api/v1/trilhaprogresso/usuario/5
        [HttpPut("usuario/{idUsuario:int}")]
        public async Task<IActionResult> Update(int idUsuario, [FromBody] TrilhaUpdateDto dto)
        {
            var trilha = await _context.TrilhasProgresso
                .FirstOrDefaultAsync(t => t.IdUsuario == idUsuario);

            if (trilha == null)
                return NotFound();

            trilha.PercentualConcluido = dto.PercentualConcluido;
            trilha.DtInicio = dto.DtInicio;
            trilha.DtUltimaAtualizacao = dto.DtUltimaAtualizacao;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/v1/trilhaprogresso/usuario/5
        [HttpDelete("usuario/{idUsuario:int}")]
        public async Task<IActionResult> Delete(int idUsuario)
        {
            var trilha = await _context.TrilhasProgresso
                .FirstOrDefaultAsync(t => t.IdUsuario == idUsuario);

            if (trilha == null)
                return NotFound();

            _context.TrilhasProgresso.Remove(trilha);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
