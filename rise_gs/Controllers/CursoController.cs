using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs;
using rise_gs.Models;
namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly RiseContext _context;
        private readonly ILogger<CursoController> _logger;

        public CursoController(RiseContext context, ILogger<CursoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/v1/curso?idUsuario=1
        [HttpGet]
        public async Task<IActionResult> GetCursos([FromQuery] int? idUsuario)
        {
            _logger.LogInformation("Listando cursos. idUsuario = {idUsuario}", idUsuario);

            var query = _context.Cursos.AsNoTracking();

            if (idUsuario.HasValue)
                query = query.Where(c => c.IdUsuario == idUsuario.Value);

            var cursos = await query.ToListAsync();

            return Ok(cursos); // 200
        }

        // GET api/v1/curso/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCursoById(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound(); // 404

            return Ok(curso); // 200
        }

        // POST api/v1/curso
        [HttpPost]
        public async Task<IActionResult> CreateCurso([FromBody] CursoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new Curso
            {
                NomeCurso = dto.NomeCurso,
                DescCurso = dto.DescCurso,
                LinkCurso = dto.LinkCurso,
                AreaCurso = dto.AreaCurso,
                IdUsuario = dto.IdUsuario
            };

            _context.Cursos.Add(entity);
            await _context.SaveChangesAsync();

            var result = new CursoCreateDto
            {
                IdCurso = entity.IdCurso,
                NomeCurso = entity.NomeCurso,
                DescCurso = entity.DescCurso,
                LinkCurso = entity.LinkCurso,
                AreaCurso = entity.AreaCurso,
                IdUsuario = entity.IdUsuario
            };

            return CreatedAtAction(
                nameof(GetCursoById),
                new { id = entity.IdCurso },
                result
            );
        }

        // PUT api/v1/curso/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCurso(int id, [FromBody] CursoUpdateDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound(); // 404

            curso.NomeCurso = dto.NomeCurso;
            curso.DescCurso = dto.DescCurso;
            curso.LinkCurso = dto.LinkCurso;
            curso.AreaCurso = dto.AreaCurso;

            await _context.SaveChangesAsync();
            return NoContent(); // 204
        }

        // DELETE api/v1/curso/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound(); // 404

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return NoContent(); // 204
        }
    }
}
