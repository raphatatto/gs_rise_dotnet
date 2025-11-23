using System.Text.Json.Serialization;

namespace rise_gs.DTOs
{
    public class AiCurriculoFeedbackRequestDto
    {
        public int IdUsuario { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CargoObjetivo { get; set; }
        public string? Resumo { get; set; }
        public List<string> Skills { get; set; } = new();
        public object? Experiences { get; set; }
        public object? Education { get; set; }
        public object? Projects { get; set; }
        public object? Certs { get; set; }
        public object? Links { get; set; }
        public int CompletenessApp { get; set; }
    }

    public class AiCurriculoFeedbackResponseDto
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("summarySuggested")]
        public string? SummarySuggested { get; set; }

        [JsonPropertyName("gaps")]
        public List<string> Gaps { get; set; } = new();

        [JsonPropertyName("suggestedBullets")]
        public List<SuggestedBulletsBlock> SuggestedBullets { get; set; } = new();

        [JsonPropertyName("recommendedCourses")]
        public List<RecommendedCourseBlock> RecommendedCourses { get; set; } = new();

        [JsonPropertyName("interviewPrep")]
        public InterviewPrepBlock? InterviewPrep { get; set; }

        [JsonPropertyName("raw")]
        public string? Raw { get; set; }
    }

    public class SuggestedBulletsBlock
    {
        [JsonPropertyName("section")]
        public string? Section { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("bullets")]
        public List<string> Bullets { get; set; } = new();
    }

    public class RecommendedCourseBlock
    {
        [JsonPropertyName("idCurso")]
        public int IdCurso { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }

    public class InterviewPrepBlock
    {
        [JsonPropertyName("questions")]
        public List<string> Questions { get; set; } = new();

        [JsonPropertyName("answersDraft")]
        public List<string> AnswersDraft { get; set; } = new();
    }
}
