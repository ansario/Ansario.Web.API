using static Ansario.Web.API.Models.ToneAnalyzerModels;

namespace Ansario.Web.API.Services
{
    public interface IToneAnalyzerService
    {
        AnalyzedTone AnalyzeText(string text);
    }
}