namespace BLL.Models
{
    public class PredictionInfoModel
    {
        public string Symbol { get; set; }

        public double Probability { get; set; }

        public bool? IsCorrect { get; set; }

        public string ExpectedSymbol { get; set; }

        public override string ToString()
        {
            return $"{Symbol}: -> {Symbol} ({100 * Probability:0.00}%)";
        }
    }
}