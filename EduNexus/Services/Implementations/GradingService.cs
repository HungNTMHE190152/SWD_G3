// Services/GradingService.cs
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels;
using EduNexus.ViewModels.Assignment;
using EduNexus.ViewModels.Grading;
using EduNexus.ViewModels.Submission;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services
{
    public class GradingService : IGradingService
    {
        private readonly EduNexusContext _context;

        public GradingService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<AssignmentGradingListViewModel>> GetAssignmentsWithSubmissionsAsync()
        {
            return await _context.Assignments
                .Where(a => a.EssaySubmissions.Any())
                .Select(a => new AssignmentGradingListViewModel
                {
                    AssignmentId = a.AssignmentId,
                    AssignmentTitle = a.Title,
                    SubmissionCount = a.EssaySubmissions.Count()
                })
                .OrderByDescending(a => a.AssignmentId)
                .ToListAsync();
        }

        public async Task<List<SubmissionListViewModel>> GetSubmissionsAsync(long assignmentId)
        {
            return await _context.EssaySubmissions
                .Where(s => s.AssignmentId == assignmentId)
                .Include(s => s.Student)
                .Include(s => s.EssayResult)
                .ThenInclude(r => r.RubricScores)
                .Select(s => new SubmissionListViewModel
                {
                    SubmissionId = s.SubmissionId,

                    StudentName = s.Student.FullName,

                    SubmittedAt = s.SubmittedAt ?? DateTime.UtcNow,

                    TotalScore = s.EssayResult != null
        ? s.EssayResult.TotalScore
        : null,

                    IsPublished = s.EssayResult != null &&
                  s.EssayResult.IsPublished == true,

                    Status = s.EssayResult != null &&
             s.EssayResult.IsPublished == true
        ? "Published"
        : "Not Graded"
                })
                .ToListAsync();
        }

        public async Task<GradeSubmissionViewModel?> GetGradingAsync(long submissionId)
        {
            var submission = await _context.EssaySubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.EssayResult)
                    .ThenInclude(r => r.RubricScores)
                .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

            if (submission == null) return null;

            // Lấy Rubric và Criteria
            var rubric = await _context.Rubrics
                .Include(r => r.RubricCriteria)
                .FirstOrDefaultAsync(r => r.AssignmentId == submission.AssignmentId);

            if (rubric == null)
            {
                throw new Exception("Assignment chưa có Rubric.");
            }

            // Tạo EssayResult nếu chưa có
            if (submission.EssayResult == null)
            {
                submission.EssayResult = new EssayResult
                {
                    SubmissionId = submissionId,
                    GraderId = 1, // TODO: Lấy từ Current User
                    GradedAt = DateTime.UtcNow,
                    IsPublished = false
                };
                _context.EssayResults.Add(submission.EssayResult);
                await _context.SaveChangesAsync();
            }

            var items = new List<RubricCriterionGradeViewModel>();

            foreach (var criterion in rubric.RubricCriteria.OrderBy(c => c.DisplayOrder))
            {
                var existingScore = submission.EssayResult.RubricScores
                    .FirstOrDefault(rs => rs.CriterionId == criterion.CriterionId);

                items.Add(new RubricCriterionGradeViewModel
                {
                    CriterionId = criterion.CriterionId,
                    CriterionName = criterion.CriterionName,
                    Description = criterion.Description,
                    Weight = criterion.Weight,
                    MaxScore = criterion.MaxScore,
                    Score = existingScore?.Score ?? 0,
                    Comment = existingScore?.Comment
                });
            }

            return new GradeSubmissionViewModel
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = submission.Assignment.Title,
                StudentName = submission.Student.FullName,
                SubmissionText = submission.SubmissionText,

                RubricId = rubric.RubricId,
                RubricName = rubric.RubricName,

                TotalScore = submission.EssayResult?.TotalScore ?? 0,

                Criteria = items
            };
        }

        public async Task SaveGradeAsync(GradeSubmissionViewModel model)
        {
            var submission = await _context.EssaySubmissions
                .Include(s => s.EssayResult)
                    .ThenInclude(r => r.RubricScores)
                .FirstOrDefaultAsync(s => s.SubmissionId == model.SubmissionId);

            if (submission?.EssayResult == null)
                throw new InvalidOperationException("Không tìm thấy EssayResult.");

            decimal totalScore = 0;

            foreach (var item in model.Criteria)
            {
                if (item.Score > item.MaxScore || item.Score < 0)
                    throw new InvalidOperationException($"Điểm của '{item.CriterionName}' phải nằm trong khoảng 0 - {item.MaxScore}");

                var existingScore = submission.EssayResult.RubricScores
                    .FirstOrDefault(rs => rs.CriterionId == item.CriterionId);

                if (existingScore != null)
                {
                    existingScore.Score = item.Score;
                    existingScore.Comment = item.Comment;
                }
                else
                {
                    submission.EssayResult.RubricScores.Add(new RubricScore
                    {
                        ResultId = submission.EssayResult.ResultId,
                        CriterionId = item.CriterionId,
                        Score = item.Score,
                        Comment = item.Comment
                    });
                }

                totalScore += item.Score;
            }

            submission.EssayResult.TotalScore = totalScore;
            //submission.EssayResult.IsPublished = model.IsPublished;
            submission.EssayResult.GradedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task PublishGradeAsync(long submissionId)
        {
            var result = await _context.EssayResults
                .FirstOrDefaultAsync(r => r.SubmissionId == submissionId);

            if (result != null)
            {
                result.IsPublished = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnpublishGradeAsync(long submissionId)
        {
            var result = await _context.EssayResults
                .FirstOrDefaultAsync(r => r.SubmissionId == submissionId);

            if (result != null)
            {
                result.IsPublished = false;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<GradeSubmissionViewModel?> GetSubmissionForGradingAsync(long submissionId)
        {
            var submission = await _context.EssaySubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.EssayResult)
                    .ThenInclude(r => r.RubricScores)
                .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

            if (submission == null) return null;

            var rubric = await _context.Rubrics
                .Include(r => r.RubricCriteria)
                .FirstOrDefaultAsync(r => r.AssignmentId == submission.AssignmentId);

            if (rubric == null) return null;

            // Tạo EssayResult nếu chưa tồn tại
            if (submission.EssayResult == null)
            {
                submission.EssayResult = new EssayResult
                {
                    SubmissionId = submissionId,
                    GraderId = 1, // TODO: Lấy từ Current User
                    GradedAt = DateTime.UtcNow,
                    IsPublished = false
                };
                _context.EssayResults.Add(submission.EssayResult);
                await _context.SaveChangesAsync();
            }

            var criteria = rubric.RubricCriteria
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new RubricCriterionGradeViewModel
                {
                    CriterionId = c.CriterionId,
                    CriterionName = c.CriterionName,
                    Description = c.Description,
                    Weight = c.Weight,
                    MaxScore = c.MaxScore,
                    Score = submission.EssayResult.RubricScores
                        .FirstOrDefault(rs => rs.CriterionId == c.CriterionId)?.Score ?? 0,
                    Comment = submission.EssayResult.RubricScores
                        .FirstOrDefault(rs => rs.CriterionId == c.CriterionId)?.Comment
                })
                .ToList();

            return new GradeSubmissionViewModel
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = submission.Assignment.Title,
                StudentName = submission.Student.FullName ?? submission.Student.FullName ?? "",
                SubmissionText = submission.SubmissionText,
                RubricId = rubric.RubricId,
                RubricName = rubric.RubricName,
                TotalScore = submission.EssayResult.TotalScore ?? criteria.Sum(x => x.Score),
                Criteria = criteria
            };
        }

        public async Task SaveGradeAsync(GradeSubmissionViewModel model, long graderId)
        {
            var submission = await _context.EssaySubmissions
                .Include(s => s.EssayResult)
                    .ThenInclude(r => r.RubricScores)
                .FirstOrDefaultAsync(s => s.SubmissionId == model.SubmissionId);

            if (submission?.EssayResult == null)
                throw new InvalidOperationException("Không tìm thấy thông tin chấm điểm.");

            decimal totalScore = 0;

            foreach (var item in model.Criteria)
            {
                if (item.Score < 0 || item.Score > item.MaxScore)
                    throw new InvalidOperationException($"Điểm của '{item.CriterionName}' phải nằm trong khoảng 0 - {item.MaxScore}");

                var existing = submission.EssayResult.RubricScores
                    .FirstOrDefault(rs => rs.CriterionId == item.CriterionId);

                if (existing != null)
                {
                    existing.Score = item.Score;
                    existing.Comment = item.Comment;
                }
                else
                {
                    submission.EssayResult.RubricScores.Add(new RubricScore
                    {
                        ResultId = submission.EssayResult.ResultId,
                        CriterionId = item.CriterionId,
                        Score = item.Score,
                        Comment = item.Comment
                    });
                }

                totalScore += item.Score;
            }

            submission.EssayResult.TotalScore = totalScore;
            submission.EssayResult.GraderId = graderId;
            submission.EssayResult.GradedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

    }
}