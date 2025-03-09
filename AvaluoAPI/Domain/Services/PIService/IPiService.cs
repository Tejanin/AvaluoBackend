using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;

namespace AvaluoAPI.Domain.Services.PIService
{
    public interface IPiService
    {
        Task<PIViewModel> GetPI(int id);
        Task<IEnumerable<PIViewModel>> GetPIsBySO(int so);

    }
}
