using Microsoft.ML.Data;

namespace rise_gs.ML
{
    // Dados de entrada do modelo
    public class SentimentData
    {
        [LoadColumn(0)]
        public bool Label { get; set; }  

        [LoadColumn(1)]
        public string Text { get; set; } = string.Empty;
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Score { get; set; }
    }
}
