using EduNexus.ViewModels.AssignmentQuestion;

namespace EduNexus.Services.Interfaces;

public interface IAssignmentQuestionService
{
    Task<AssignmentQuestionManagementViewModel>
        GetAssignmentQuestionsAsync(long assignmentId);

    Task AddQuestionAsync(long assignmentId, long questionId);

    Task RemoveQuestionAsync(long assignmentQuestionId);

    Task UpdateQuestionAsync(AssignmentQuestionItemViewModel model);

    Task UpdateQuestionsAsync(List<AssignmentQuestionItemViewModel> questions);

}