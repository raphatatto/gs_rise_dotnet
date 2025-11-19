using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rise_gs.DTOs;
using rise_gs.Models;

namespace rise_gs.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class TrilhaObjetivoController : ControllerBase
	{
		private readonly RiseContext _context;
		private readonly ILogger<TrilhaObjetivoController> _logger;

		public TrilhaObjetivoController(RiseContext context, ILogger<TrilhaObjetivoController> logger)
		{
			_context = context;
			_logger = logger;
		}

		// GET api/v1/trilhaobjetivo?idTrilha=1
		[HttpGet]
		public async Task<IActionResult> GetObjetivos([FromQuery] int? idTrilha)
		{
			_logger.LogInformation("Listando objetivos. idTrilha = {idTrilha}", idTrilha);

			var query = _context.TrilhasObjetivos.AsNoTracking();

			if (idTrilha.HasValue)
				query = query.Where(o => o.IdTrilha == idTrilha.Value);

			var objetivos = await query.ToListAsync();

			var result = objetivos.Select(o => new TrilhaObjetivoDto
			{
				IdObjetivo = o.IdObjetivo,
				IdTrilha = o.IdTrilha,
				TituloObjetivo = o.TituloObjetivo,
				CategoriaObjetivo = o.CategoriaObjetivo,
				DataPlanejada = o.DataPlanejada,
				Concluido = o.Concluido,
				DataConclusao = o.DataConclusao,
				DtCriacao = o.DtCriacao
			});

			return Ok(result);
		}

		// GET api/v1/trilhaobjetivo/5
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var objetivo = await _context.TrilhasObjetivos
				.AsNoTracking()
				.FirstOrDefaultAsync(o => o.IdObjetivo == id);

			if (objetivo == null)
				return NotFound();

			var dto = new TrilhaObjetivoDto
			{
				IdObjetivo = objetivo.IdObjetivo,
				IdTrilha = objetivo.IdTrilha,
				TituloObjetivo = objetivo.TituloObjetivo,
				CategoriaObjetivo = objetivo.CategoriaObjetivo,
				DataPlanejada = objetivo.DataPlanejada,
				Concluido = objetivo.Concluido,
				DataConclusao = objetivo.DataConclusao,
				DtCriacao = objetivo.DtCriacao
			};

			return Ok(dto);
		}

		// POST api/v1/trilhaobjetivo
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] TrilhaObjetivoCreateDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var objetivo = new TrilhaObjetivo
			{
				IdTrilha = dto.IdTrilha,
				TituloObjetivo = dto.TituloObjetivo,
				CategoriaObjetivo = dto.CategoriaObjetivo,
				DataPlanejada = dto.DataPlanejada,
				Concluido = "N",                  // padrão: não concluído
				DtCriacao = DateTime.UtcNow
			};

			_context.TrilhasObjetivos.Add(objetivo);
			await _context.SaveChangesAsync();

			var result = new TrilhaObjetivoDto
			{
				IdObjetivo = objetivo.IdObjetivo,
				IdTrilha = objetivo.IdTrilha,
				TituloObjetivo = objetivo.TituloObjetivo,
				CategoriaObjetivo = objetivo.CategoriaObjetivo,
				DataPlanejada = objetivo.DataPlanejada,
				Concluido = objetivo.Concluido,
				DataConclusao = objetivo.DataConclusao,
				DtCriacao = objetivo.DtCriacao
			};

			return CreatedAtAction(nameof(GetById),
				new { id = objetivo.IdObjetivo },
				result);
		}

		// PUT api/v1/trilhaobjetivo/5
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, [FromBody] TrilhaObjetivoUpdateDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var objetivo = await _context.TrilhasObjetivos
				.FirstOrDefaultAsync(o => o.IdObjetivo == id);

			if (objetivo == null)
				return NotFound();

			objetivo.TituloObjetivo = dto.TituloObjetivo ?? objetivo.TituloObjetivo;
			objetivo.CategoriaObjetivo = dto.CategoriaObjetivo ?? objetivo.CategoriaObjetivo;
			objetivo.DataPlanejada = dto.DataPlanejada ?? objetivo.DataPlanejada;

			// Atualiza status de conclusão
			if (!string.IsNullOrWhiteSpace(dto.Concluido))
			{
				objetivo.Concluido = dto.Concluido;

				if (dto.Concluido == "S" && objetivo.DataConclusao == null)
				{
					// Se marcar como concluído e não tiver data, seta agora
					objetivo.DataConclusao = dto.DataConclusao ?? DateTime.UtcNow;
				}
				else if (dto.Concluido == "N")
				{
					// Se voltar para não concluído, opcionalmente zera DataConclusao
					objetivo.DataConclusao = dto.DataConclusao;
				}
			}
			else if (dto.DataConclusao.HasValue)
			{
				// Se só alterar data de conclusão (caso limite)
				objetivo.DataConclusao = dto.DataConclusao.Value;
			}

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE api/v1/trilhaobjetivo/5
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var objetivo = await _context.TrilhasObjetivos
				.FirstOrDefaultAsync(o => o.IdObjetivo == id);

			if (objetivo == null)
				return NotFound();

			_context.TrilhasObjetivos.Remove(objetivo);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
