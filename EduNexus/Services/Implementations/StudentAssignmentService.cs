using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Student;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Implementations
{
    public class StudentAssignmentService : IStudentAssignmentService
    {
        private readonly EduNexusContext _context;

        public StudentAssignmentService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<List<StudentAssignmentListViewModel>> GetAssignmentsAsync(long studentId)
        {
            var now = DateTime.UtcNow;

            return await _context.Enrollments
                .Where(e => e.StudentId == studentId && e.Status == "ACTIVE")
                .SelectMany(e => e.Course.Modules)
                .SelectMany(m => m.Assignments)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .Select(a => new
                {
                    Assignment = a,
                    Submission = a.EssaySubmissions.FirstOrDefault(s => s.StudentId == studentId)
                })
                .Select(x => new StudentAssignmentListViewModel
                {
                    AssignmentId = x.Assignment.AssignmentId,
                    Title = x.Assignment.Title,
                    CourseName = x.Assignment.Module.Course.Title,
                    ModuleName = x.Assignment.Module.ModuleName,
                    AssignmentType = x.Assignment.AssignmentType,
                    TotalScore = x.Assignment.TotalScore,
                    OpenDate = x.Assignment.OpenDate,
                    DueDate = x.Assignment.DueDate,

                    // Tính Assignment Status
                    AssignmentStatus = x.Assignment.OpenDate > now ? "Not Open" :
                                       (x.Assignment.DueDate < now ? "Closed" : "Open"),

                    // Tính Submission Status
                    SubmissionStatus = x.Submission == null ? "Not Submitted" :
                   (x.Submission.Status == "SUBMITTED" ? "Submitted" : "Draft"),

                    IsSubmitted = x.Submission.Status == "SUBMITTED",
                    IsOpen = x.Assignment.Status == "PUBLISHED" &&
                             x.Assignment.OpenDate <= now &&
                             (x.Assignment.DueDate == null || x.Assignment.DueDate >= now),
                    IsClosed = x.Assignment.DueDate < now
                })
                .OrderBy(a => a.AssignmentStatus)
                .ThenBy(a => a.DueDate)
                .ToListAsync();
        }
        public async Task<StudentAssignmentDetailViewModel?> GetAssignmentDetailAsync(long assignmentId, long studentId)
        {
            var now = DateTime.UtcNow;

            var data = await _context.Assignments
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .Include(a => a.AssignmentQuestions)
                    .ThenInclude(aq => aq.Question)
                .Include(a => a.EssaySubmissions)
                .Where(a => a.AssignmentId == assignmentId)
                .Select(a => new
                {
                    Assignment = a,
                    Submission = a.EssaySubmissions.FirstOrDefault(s => s.StudentId == studentId),
                    IsEnrolled = _context.Enrollments.Any(e =>
                        e.StudentId == studentId &&
                        e.CourseId == a.Module.Course.CourseId &&
                        e.Status == "ACTIVE")
                })
                .FirstOrDefaultAsync();

            if (data == null || !data.IsEnrolled)
                return null;

            var assignment = data.Assignment;

            var model = new StudentAssignmentDetailViewModel
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                Description = assignment.Description,
                CourseName = assignment.Module.Course.Title,
                ModuleName = assignment.Module.ModuleName,
                AssignmentType = assignment.AssignmentType,
                TotalScore = assignment.TotalScore,
                OpenDate = assignment.OpenDate,
                DueDate = assignment.DueDate,
                AllowLateSubmission = assignment.AllowLateSubmission,
                Status = assignment.Status ?? "Draft",

                // Status
                IsOpen = assignment.Status == "PUBLISHED" &&
                         assignment.OpenDate <= now &&
                         (assignment.DueDate == null || assignment.DueDate >= now),
                IsClosed = assignment.DueDate < now,
                IsSubmitted = data.Submission?.Status == "SUBMITTED",
                SubmissionStatus = data.Submission?.Status switch
                {
                    "SUBMITTED" => "Submitted",
                    "DRAFT" => "Draft",
                    _ => "Not Submitted"
                }
            };

            // Load Questions (nếu có)
            model.Questions = assignment.AssignmentQuestions
                .OrderBy(aq => aq.DisplayOrder)
                .Select(aq => new QuestionItemViewModel
                {
                    QuestionId = aq.QuestionId,
                    QuestionContent = aq.Question.QuestionContent,
                    QuestionType = aq.Question.QuestionType,
                    Difficulty = aq.Question.Difficulty,
                    Score = aq.Score
                })
                .ToList();

            return model;
        }
        public async Task<StudentAssignmentWorkspaceViewModel?> StartAssignmentAsync(long assignmentId, long studentId)
        {
            var now = DateTime.UtcNow;

            var assignment = await _context.Assignments
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .Include(a => a.AssignmentQuestions)
                    .ThenInclude(aq => aq.Question)
                .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

            if (assignment == null) return null;

            // Kiểm tra Enrollment
            bool isEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId
                            && e.CourseId == assignment.Module.CourseId
                            && e.Status == "ACTIVE");

            if (!isEnrolled) return null;

            // Kiểm tra thời gian
            if (assignment.Status != "PUBLISHED")
                return null;
            bool isOpen = assignment.OpenDate <= now && (assignment.DueDate == null || assignment.DueDate >= now);
            if (!isOpen && !(assignment.AllowLateSubmission == true && assignment.DueDate < now))
                return null; // Không cho start

            // Tìm Submission hiện có
            var submission = await _context.EssaySubmissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);

            if (submission == null)
            {
                submission = new EssaySubmission
                {
                    AssignmentId = assignmentId,
                    StudentId = studentId,
                    Status = "DRAFT",
                    SubmittedAt = null,
                    GradingStatus = "PENDING"
                };
                _context.EssaySubmissions.Add(submission);
                await _context.SaveChangesAsync();
            }

            var model = new StudentAssignmentWorkspaceViewModel
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = assignment.AssignmentId,
                StudentId = studentId,
                Title = assignment.Title,
                Description = assignment.Description,
                AssignmentType = assignment.AssignmentType,
                OpenDate = assignment.OpenDate,
                DueDate = assignment.DueDate,
                AllowLateSubmission = assignment.AllowLateSubmission ?? false,
                SubmissionText = submission.SubmissionText ?? "",
                LastSaved = submission.SubmittedAt,
                Status = submission.Status ?? "DRAFT"
            };

            // Load Questions nếu là Quiz hoặc Mixed
            if (assignment.AssignmentType.Contains("QUIZ") || assignment.AssignmentType.Contains("MIXED"))
            {
                model.Questions = assignment.AssignmentQuestions
                    .OrderBy(aq => aq.DisplayOrder)
                    .Select(aq => new QuestionAnswerViewModel
                    {
                        QuestionId = aq.QuestionId,
                        QuestionContent = aq.Question.QuestionContent,
                        QuestionType = aq.Question.QuestionType,
                        DisplayOrder = aq.DisplayOrder ?? 0,
                        Answer = "" // TODO: Load answer nếu có lưu
                    })
                    .ToList();
            }

            return model;
        }

        public async Task SaveDraftAsync(StudentAssignmentWorkspaceViewModel model, long studentId)
        {
            var submission = await _context.EssaySubmissions
                .FirstOrDefaultAsync(s => s.SubmissionId == model.SubmissionId && s.StudentId == studentId);

            if (submission == null)
                throw new InvalidOperationException("Không tìm thấy bài làm.");

            submission.SubmissionText = model.SubmissionText;
            submission.Status = "DRAFT";
            submission.SubmittedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        public async Task<bool> SubmitAssignmentAsync(long assignmentId, long studentId)
        {
            var now = DateTime.UtcNow;

            var submission = await _context.EssaySubmissions
                .Include(s => s.Assignment)
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);

            if (submission == null)
                return false;

            // Kiểm tra đã submit chưa
            if (submission.Status == "SUBMITTED")
                return false;

            // Kiểm tra thời gian
            var assignment = submission.Assignment;
            bool isLate = assignment.DueDate < now;
            if (isLate && assignment.AllowLateSubmission != true)
                return false;

            // Submit
            submission.Status = "SUBMITTED";
            submission.SubmittedAt = now;
            submission.GradingStatus = "PENDING";

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<StudentSubmissionHistoryViewModel>> GetSubmissionHistoryAsync(long studentId)
        {
            return await _context.EssaySubmissions
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Module)
                        .ThenInclude(m => m.Course)
                .Select(s => new StudentSubmissionHistoryViewModel
                {
                    SubmissionId = s.SubmissionId,
                    AssignmentId = s.AssignmentId,
                    AssignmentTitle = s.Assignment.Title,
                    CourseName = s.Assignment.Module.Course.Title,
                    ModuleName = s.Assignment.Module.ModuleName,
                    SubmittedAt = s.SubmittedAt,
                    SubmissionStatus = s.Status ?? "DRAFT",
                    GradingStatus = s.GradingStatus ?? "PENDING",
                    IsDraft = s.Status == "DRAFT"
                })
                .OrderByDescending(s => s.SubmittedAt ?? DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<StudentSubmissionDetailViewModel?> GetSubmissionDetailAsync(long submissionId, long studentId)
        {
            var submission = await _context.EssaySubmissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Module)
                        .ThenInclude(m => m.Course)
                .Include(s => s.SubmissionAttachments)
                .FirstOrDefaultAsync(s => s.SubmissionId == submissionId && s.StudentId == studentId);

            if (submission == null) return null;

            return new StudentSubmissionDetailViewModel
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = submission.Assignment.Title,
                AssignmentDescription = submission.Assignment.Description,
                CourseName = submission.Assignment.Module.Course.Title,
                ModuleName = submission.Assignment.Module.ModuleName,
                SubmissionText = submission.SubmissionText,
                SubmittedAt = submission.SubmittedAt,
                SubmissionStatus = submission.Status ?? "DRAFT",
                GradingStatus = submission.GradingStatus ?? "PENDING",
                IsDraft = submission.Status == "DRAFT",
                Attachments = submission.SubmissionAttachments
                    .Select(a => new SubmissionAttachmentViewModel
                    {
                        AttachmentId = a.AttachmentId,
                        FileName = a.FileName,
                        FileUrl = a.FileUrl,
                        UploadedAt = a.UploadedAt
                    })
                    .ToList()
            };
        }
        public async Task<EssaySubmission?> GetLatestSubmissionAsync(long assignmentId, long studentId)
        {
            return await _context.EssaySubmissions
                .OrderByDescending(s => s.SubmissionId)
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);
        }


    }
}