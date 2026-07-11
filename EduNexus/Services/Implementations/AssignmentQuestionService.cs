    using EduNexus.Data;
    using EduNexus.Models;
    using EduNexus.Services.Interfaces;
    using EduNexus.ViewModels.AssignmentQuestion;
    using Microsoft.EntityFrameworkCore;

    namespace EduNexus.Services.Implementations
    {
        public class AssignmentQuestionService : IAssignmentQuestionService
        {
            private readonly EduNexusContext _context;

            public AssignmentQuestionService(EduNexusContext context)
            {
                _context = context;
            }

            public async Task<AssignmentQuestionManagementViewModel> GetAssignmentQuestionsAsync(long assignmentId)
            {
                Console.WriteLine(assignmentId);
                var assignment = await _context.Assignments
                    .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

                if (assignment == null)
                {
                    throw new Exception("Assignment not found.");
                }

                var model = new AssignmentQuestionManagementViewModel
                {
                    AssignmentId = assignment.AssignmentId,
                    AssignmentTitle = assignment.Title
                };

                // Các Question đã có trong Assignment
                model.CurrentQuestions = await _context.AssignmentQuestions
                    .Where(aq => aq.AssignmentId == assignmentId)
                    .Include(aq => aq.Question)
                    .Select(aq => new AssignmentQuestionItemViewModel
                    {
                        AssignmentQuestionId = aq.AssignmentQuestionId,
                        QuestionId = aq.QuestionId,
                        QuestionContent = aq.Question.QuestionContent,
                        QuestionType = aq.Question.QuestionType,
                        Difficulty = aq.Question.Difficulty,
                        Score = aq.Score,
                        DisplayOrder = aq.DisplayOrder
                    })
                    .ToListAsync();

                // Các Question chưa được thêm vào Assignment
                model.AvailableQuestions = await _context.Questions
                    .Where(q => !_context.AssignmentQuestions
                        .Any(aq =>
                            aq.AssignmentId == assignmentId &&
                            aq.QuestionId == q.QuestionId))
                    .Select(q => new AssignmentQuestionItemViewModel
                    {
                        QuestionId = q.QuestionId,
                        QuestionContent = q.QuestionContent,
                        QuestionType = q.QuestionType,
                        Difficulty = q.Difficulty
                    })
                    .ToListAsync();

                return model;
            }

            public async Task AddQuestionAsync(long assignmentId, long questionId)
            {
                bool existed = await _context.AssignmentQuestions
                    .AnyAsync(x =>
                        x.AssignmentId == assignmentId &&
                        x.QuestionId == questionId);

                if (existed)
                {
                    return;
                }

                int displayOrder = await _context.AssignmentQuestions
                    .Where(x => x.AssignmentId == assignmentId)
                    .CountAsync();

                AssignmentQuestion assignmentQuestion = new AssignmentQuestion
                {
                    AssignmentId = assignmentId,
                    QuestionId = questionId,
                    Score = 1,
                    DisplayOrder = displayOrder + 1
                };

                _context.AssignmentQuestions.Add(assignmentQuestion);

                await _context.SaveChangesAsync();
            }

            public async Task RemoveQuestionAsync(long assignmentQuestionId)
            {
                var assignmentQuestion = await _context.AssignmentQuestions
                    .FirstOrDefaultAsync(x =>
                        x.AssignmentQuestionId == assignmentQuestionId);

                if (assignmentQuestion == null)
                {
                    return;
                }

                _context.AssignmentQuestions.Remove(assignmentQuestion);

                await _context.SaveChangesAsync();
            }

            public async Task UpdateQuestionAsync(AssignmentQuestionItemViewModel model)
            {
                var assignmentQuestion = await _context.AssignmentQuestions
                    .FirstOrDefaultAsync(a =>
                        a.AssignmentQuestionId == model.AssignmentQuestionId);

                if (assignmentQuestion == null)
                {
                    throw new Exception("Assignment Question not found.");
                }

                assignmentQuestion.Score = model.Score;
                assignmentQuestion.DisplayOrder = model.DisplayOrder;

                await _context.SaveChangesAsync();
            }

            public async Task UpdateQuestionsAsync(List<AssignmentQuestionItemViewModel> questions)
            {
                foreach (var item in questions)
                {
                    var entity = await _context.AssignmentQuestions
                        .FirstOrDefaultAsync(x => x.AssignmentQuestionId == item.AssignmentQuestionId);

                    if (entity == null)
                        continue;

                    entity.Score = item.Score;
                    entity.DisplayOrder = item.DisplayOrder;
                }

                await _context.SaveChangesAsync();
            }
        }
    }