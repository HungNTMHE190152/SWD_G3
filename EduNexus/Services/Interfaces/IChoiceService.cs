using System.Threading.Tasks;
using EduNexus.ViewModels.Questions;

namespace EduNexus.Services.Interfaces
{
    public interface IChoiceService
    {
        Task<ChoiceFormViewModel?> GetChoiceByIdAsync(long choiceId);
        Task<long> CreateChoiceAsync(ChoiceFormViewModel model);
        Task<bool> UpdateChoiceAsync(ChoiceFormViewModel model);
        Task<bool> DeleteChoiceAsync(long choiceId);
    }
}
