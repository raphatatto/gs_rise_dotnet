using Microsoft.ML;
using rise_gs.ML;

namespace rise_gs.Services
{
    public class SentimentService
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

        public SentimentService()
        {
            _mlContext = new MLContext();

            // Dados de treinamento em memória (só pra demonstração)
            var trainingData = new List<SentimentData>
            {
                new() { Label = false, Text = "O curso é excelente, aprendi muito." },
                new() { Label = false, Text = "Estou muito satisfeito com a plataforma." },
                new() { Label = true,  Text = "O conteúdo é muito fraco e ruim." },
                new() { Label = true,  Text = "Não gostei da experiência, foi péssima." },
                new() { Label = false, Text = "A experiência foi ótima e recomendo." },
                new() { Label = true,  Text = "Estou decepcionado com o serviço." }
            };

            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Pipeline de ML: converte texto em features e treina um classificador
            var pipeline = _mlContext.Transforms.Text.FeaturizeText(
                                outputColumnName: "Features",
                                inputColumnName: nameof(SentimentData.Text))
                            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                                labelColumnName: nameof(SentimentData.Label),
                                featureColumnName: "Features"));

            var model = pipeline.Fit(dataView);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        }

        public (string Sentimento, float Score) Predict(string text)
        {
            var input = new SentimentData { Text = text };
            var prediction = _predictionEngine.Predict(input);

            var sentimento = prediction.Prediction ? "Negativo" : "Positivo";
            return (sentimento, prediction.Score);
        }
    }
}
