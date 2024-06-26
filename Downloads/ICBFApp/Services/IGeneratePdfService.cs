using ICBFApp.Pages.Niños;
using QuestPDF.Fluent;

namespace ICBFApp.Services
{
    public interface IGeneratePdfService
    {

        Document GeneratePdfQuest();
    }
}
