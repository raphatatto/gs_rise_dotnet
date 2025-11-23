using Microsoft.AspNetCore.Mvc;
using rise_gs.Services;

namespace rise_gs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SentimentController : ControllerBase
    {
        private readonly SentimentService _sentimentService;

        public SentimentController(SentimentService sentimentService)
        {
            _sentimentService = sentimentService;
        }

        public class SentimentRequest
        {
            public string Text { get; set; } = string.Empty;
        }

        [HttpPost("analisar")]
        public IActionResult AnalisarSentimento([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Informe um texto para análise.");

            var (sentimento, score) = _sentimentService.Predict(request.Text);

            return Ok(new
            {
                texto = request.Text,
                sentimento,
                score
            });
        }
    }
}
