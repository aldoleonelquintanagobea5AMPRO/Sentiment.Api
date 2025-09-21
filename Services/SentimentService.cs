using System.Globalization;

namespace Sentiment.Api.Services
{
    public class SentimentService
    {
        private readonly string[] positive = new[] { "excelente", "genial", "fantástico", "bueno", "increíble" };
        private readonly string[] negative = new[] { "malo", "terrible", "problema", "defecto", "horrible" };

        // Devuelve "positivo", "negativo" o "neutral"
        public string Analyze(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "neutral";

            // case-insensitive: normalizamos a minúsculas (CultureInvariant)
            var lower = text.ToLower(CultureInfo.InvariantCulture);

            foreach (var p in positive)
            {
                if (lower.Contains(p)) return "positivo";
            }
            foreach (var n in negative)
            {
                if (lower.Contains(n)) return "negativo";
            }
            return "neutral";
        }
    }
}
