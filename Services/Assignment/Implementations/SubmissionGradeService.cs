using EduNexus.Data;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;
using EduNexus.Models;

namespace EduNexus.Services.Assignment.Implementations
{
    public class SubmissionGradeService : ISubmissionGradeService
    {
        private readonly EduNexusContext _context;

        public SubmissionGradeService(EduNexusContext context)
        {
            _context = context;
        }
        public async Task<SubmissionGradeViewModel?> GetSubmissionAsync(long submissionId)
        {
            return await _context.AssignmentSubmissions
                .Include(x => x.Student)
                .Include(x => x.ClassroomAssignment)
                .Include(x => x.SubmissionGrade)
                .Where(x => x.AssignmentSubmissionId == submissionId)
                .Select(x => new SubmissionGradeViewModel
                {
                    SubmissionId = x.AssignmentSubmissionId,
                    ClassroomAssignmentId = x.ClassroomAssignmentId,
                    AssignmentTitle = x.ClassroomAssignment.Title,
                    StudentName = x.Student.FullName,
                    SubmittedAt = x.SubmittedAt,
                    SubmissionText = x.SubmissionText,
                    FileUrl = x.FileUrl,
                    Score = x.SubmissionGrade != null
                        ? (double)x.SubmissionGrade.Score
                        : 0,
                    Feedback = x.SubmissionGrade != null
                        ? x.SubmissionGrade.Feedback
                        : ""
                })
                .FirstOrDefaultAsync();
        }

        public async Task SaveGradeAsync(
    SubmissionGradeViewModel model,
    long teacherId)
        {
            var grade = await _context.SubmissionGrades
                .FirstOrDefaultAsync(x =>
                    x.AssignmentSubmissionId == model.SubmissionId);

            if (grade == null)
            {
                grade = new SubmissionGrade
                {
                    AssignmentSubmissionId = model.SubmissionId,
                    TeacherId = teacherId,
                    Score = (decimal)model.Score,
                    Feedback = model.Feedback,
                    GradedAt = DateTime.Now
                };

                _context.SubmissionGrades.Add(grade);
            }
            else
            {
                grade.Score = (decimal)model.Score;
                grade.Feedback = model.Feedback;
                grade.TeacherId = teacherId;
                grade.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
