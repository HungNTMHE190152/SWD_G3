using EduNexus.Areas.Admin.ViewModels.CourseReview;
using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class CourseReviewService : ICourseReviewService
    {
        private readonly EduNexusContext _context;

        public CourseReviewService(
            EduNexusContext context)
        {
            _context = context;
        }

        public async Task<CourseReviewIndexViewModel>
            GetCoursesAsync(
                CourseReviewFilterViewModel filter)
        {
            NormalizeFilter(filter);

            IQueryable<Course> query =
                _context.Courses
                    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(
                filter.SearchText))
            {
                string keyword =
                    filter.SearchText.Trim();

                query = query.Where(course =>
                    course.CourseCode.Contains(keyword)
                    || course.Title.Contains(keyword)
                    || course.CreatedByNavigation
                        .FullName.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(
                filter.Status))
            {
                query = query.Where(course =>
                    course.Status == filter.Status);
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(course =>
                    course.CategoryId ==
                    filter.CategoryId.Value);
            }

            int totalItems =
                await query.CountAsync();

            int totalPages =
                totalItems == 0
                    ? 1
                    : (int)Math.Ceiling(
                        totalItems /
                        (double)filter.PageSize);

            if (filter.Page > totalPages)
            {
                filter.Page = totalPages;
            }

            List<CourseReviewListItemViewModel> courses =
                await query
                    .OrderByDescending(course =>
                        course.SubmittedAt
                        ?? course.UpdatedAt
                        ?? course.CreatedAt)
                    .Skip(
                        (filter.Page - 1)
                        * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(course =>
                        new CourseReviewListItemViewModel
                        {
                            CourseId =
                                course.CourseId,

                            CourseCode =
                                course.CourseCode,

                            Title =
                                course.Title,

                            CategoryName =
                                course.Category.CategoryName,

                            SmeName =
                                course.CreatedByNavigation
                                    .FullName,

                            Status =
                                course.Status,

                            ThumbnailUrl =
                                course.ThumbnailUrl,

                            CreatedAt =
                                course.CreatedAt,

                            UpdatedAt =
                                course.UpdatedAt,

                            SubmittedAt =
                                course.SubmittedAt,

                            ModuleCount =
                                _context.Modules.Count(module =>
                                    module.CourseId ==
                                    course.CourseId),

                            LessonCount =
                                (
                                    from lesson in
                                        _context.Lessons
                                    join module in
                                        _context.Modules
                                        on lesson.ModuleId
                                        equals module.ModuleId
                                    where module.CourseId ==
                                        course.CourseId
                                    select lesson.LessonId
                                ).Count(),

                            QuizTemplateCount =
                                _context.QuizTemplates.Count(
                                    quiz =>
                                        quiz.CourseId ==
                                        course.CourseId),

                            AssignmentTemplateCount =
                                _context.AssignmentTemplates.Count(
                                    assignment =>
                                        assignment.CourseId ==
                                        course.CourseId)
                        })
                    .ToListAsync();

            List<CourseReviewCategoryOptionViewModel>
                categories =
                    await _context.Categories
                        .AsNoTracking()
                        .OrderBy(category =>
                            category.CategoryName)
                        .Select(category =>
                            new CourseReviewCategoryOptionViewModel
                            {
                                CategoryId =
                                    category.CategoryId,

                                CategoryName =
                                    category.CategoryName
                            })
                        .ToListAsync();

            int pendingCount =
                await _context.Courses
                    .AsNoTracking()
                    .CountAsync(course =>
                        course.Status ==
                        CourseStatuses.PendingReview);

            int approvedCount =
                await _context.Courses
                    .AsNoTracking()
                    .CountAsync(course =>
                        course.Status ==
                        CourseStatuses.Approved);

            int rejectedCount =
                await _context.Courses
                    .AsNoTracking()
                    .CountAsync(course =>
                        course.Status ==
                        CourseStatuses.Rejected);

            return new CourseReviewIndexViewModel
            {
                Filter = filter,
                Courses = courses,
                Categories = categories,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PendingCount = pendingCount,
                ApprovedCount = approvedCount,
                RejectedCount = rejectedCount
            };
        }

        private static void NormalizeFilter(
            CourseReviewFilterViewModel filter)
        {
            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.PageSize < 5
                || filter.PageSize > 100)
            {
                filter.PageSize = 10;
            }

            string[] validStatuses =
            {
                CourseStatuses.Draft,
                CourseStatuses.PendingReview,
                CourseStatuses.Approved,
                CourseStatuses.Rejected,
                CourseStatuses.Archived
            };

            if (!string.IsNullOrWhiteSpace(
                    filter.Status)
                && !validStatuses.Contains(
                    filter.Status))
            {
                filter.Status = null;
            }
        }
        public async Task<CourseReviewDetailsViewModel?> GetDetailsAsync(
    long courseId)
        {
            CourseReviewDetailsViewModel? viewModel =
                await (
                    from course in _context.Courses.AsNoTracking()

                    join category in _context.Categories.AsNoTracking()
                        on course.CategoryId equals category.CategoryId

                    join sme in _context.Users.AsNoTracking()
                        on course.CreatedBy equals sme.UserId

                    where course.CourseId == courseId

                    select new CourseReviewDetailsViewModel
                    {
                        CourseId = course.CourseId,
                        CourseCode = course.CourseCode,
                        Title = course.Title,
                        Description = course.Description,
                        ThumbnailUrl = course.ThumbnailUrl,
                        Status = course.Status,

                        CategoryName = category.CategoryName,

                        SmeName = sme.FullName,
                        SmeEmail = sme.Email,

                        CreatedAt = course.CreatedAt,
                        UpdatedAt = course.UpdatedAt,
                        SubmittedAt = course.SubmittedAt
                    }
                ).FirstOrDefaultAsync();

            if (viewModel == null)
            {
                return null;
            }

            List<CourseReviewModuleViewModel> modules =
                await _context.Modules
                    .AsNoTracking()
                    .Where(module =>
                        module.CourseId == courseId)
                    .OrderBy(module =>
                        module.DisplayOrder)
                    .Select(module =>
                        new CourseReviewModuleViewModel
                        {
                            ModuleId = module.ModuleId,
                            Title = module.Title,
                            Description = module.Description,
                            DisplayOrder = module.DisplayOrder
                        })
                    .ToListAsync();

            List<long> moduleIds =
                modules
                    .Select(module => module.ModuleId)
                    .ToList();

            List<CourseReviewLessonViewModel> lessons =
                new();

            if (moduleIds.Count > 0)
            {
                lessons =
                    await _context.Lessons
                        .AsNoTracking()
                        .Where(lesson =>
                            moduleIds.Contains(lesson.ModuleId))
                        .OrderBy(lesson =>
                            lesson.DisplayOrder)
                        .Select(lesson =>
                            new CourseReviewLessonViewModel
                            {
                                LessonId = lesson.LessonId,
                                ModuleId = lesson.ModuleId,
                                Title = lesson.Title,
                                Content = lesson.Content,
                                LessonType = lesson.LessonType,
                                DisplayOrder = lesson.DisplayOrder,
                                Status = lesson.Status,
                                EstimatedMinutes =
                                    lesson.EstimatedMinutes,

                                ResourceCount =
                                    _context.LessonResources.Count(
                                        resource =>
                                            resource.LessonId ==
                                            lesson.LessonId)
                            })
                        .ToListAsync();
            }

            foreach (CourseReviewModuleViewModel module in modules)
            {
                module.Lessons =
                    lessons
                        .Where(lesson =>
                            lesson.ModuleId == module.ModuleId)
                        .OrderBy(lesson =>
                            lesson.DisplayOrder)
                        .ToList();
            }

            viewModel.Modules = modules;

            viewModel.QuizTemplates =
                await _context.QuizTemplates
                    .AsNoTracking()
                    .Where(quiz =>
                        quiz.CourseId == courseId)
                    .OrderBy(quiz =>
                        quiz.CreatedAt)
                    .Select(quiz =>
                        new CourseReviewQuizViewModel
                        {
                            QuizTemplateId =
                                quiz.QuizTemplateId,

                            Title =
                                quiz.Title,

                            Description =
                                quiz.Description,

                            DefaultDurationMinutes =
                                quiz.DefaultDurationMinutes,

                            DefaultPassingScore =
                                quiz.DefaultPassingScore,

                            DefaultMaxAttempts =
                                quiz.DefaultMaxAttempts,

                            IsActive =
                                quiz.IsActive,

                            QuestionCount =
                                _context.QuizTemplateQuestions.Count(
                                    item =>
                                        item.QuizTemplateId ==
                                        quiz.QuizTemplateId)
                        })
                    .ToListAsync();

            viewModel.AssignmentTemplates =
                await _context.AssignmentTemplates
                    .AsNoTracking()
                    .Where(assignment =>
                        assignment.CourseId == courseId)
                    .OrderBy(assignment =>
                        assignment.CreatedAt)
                    .Select(assignment =>
                        new CourseReviewAssignmentViewModel
                        {
                            AssignmentTemplateId =
                                assignment.AssignmentTemplateId,

                            Title =
                                assignment.Title,

                            Description =
                                assignment.Description,

                            Instructions =
                                assignment.Instructions,

                            TotalScore =
                                assignment.TotalScore,

                            IsActive =
                                assignment.IsActive,

                            HasRubric =
                                _context.Rubrics.Any(rubric =>
                                    rubric.AssignmentTemplateId ==
                                    assignment.AssignmentTemplateId),

                            RubricCriterionCount =
                                (
                                    from rubric in _context.Rubrics
                                    join criterion in
                                        _context.RubricCriteria
                                        on rubric.RubricId
                                        equals criterion.RubricId
                                    where rubric.AssignmentTemplateId ==
                                        assignment.AssignmentTemplateId
                                    select criterion.RubricCriterionId
                                ).Count()
                        })
                    .ToListAsync();

            viewModel.ReviewHistory =
                await (
                    from review in
                        _context.CourseReviews.AsNoTracking()

                    join reviewer in
                        _context.Users.AsNoTracking()
                        on review.ReviewedBy
                        equals reviewer.UserId

                    where review.CourseId == courseId

                    orderby review.ReviewedAt descending

                    select new CourseReviewHistoryItemViewModel
                    {
                        CourseReviewId =
                            review.CourseReviewId,

                        Decision =
                            review.Decision,

                        ReviewComment =
                            review.ReviewComment,

                        ReviewerName =
                            reviewer.FullName,

                        ReviewedAt =
                            review.ReviewedAt
                    }
                ).ToListAsync();

            BuildContentWarnings(viewModel);

            return viewModel;
        }

        private static void BuildContentWarnings(
    CourseReviewDetailsViewModel viewModel)
        {
            if (viewModel.ModuleCount == 0)
            {
                viewModel.ContentWarnings.Add(
                    "This course does not contain any modules.");
            }

            if (viewModel.LessonCount == 0)
            {
                viewModel.ContentWarnings.Add(
                    "This course does not contain any lessons.");
            }

            if (viewModel.DraftLessonCount > 0)
            {
                viewModel.ContentWarnings.Add(
                    $"This course contains "
                    + $"{viewModel.DraftLessonCount} draft lesson(s).");
            }

            foreach (CourseReviewModuleViewModel module
                in viewModel.Modules)
            {
                if (module.Lessons.Count == 0)
                {
                    viewModel.ContentWarnings.Add(
                        $"Module \"{module.Title}\" does not "
                        + "contain any lessons.");
                }
            }

            foreach (CourseReviewQuizViewModel quiz
                in viewModel.QuizTemplates)
            {
                if (quiz.QuestionCount == 0)
                {
                    viewModel.ContentWarnings.Add(
                        $"Quiz \"{quiz.Title}\" does not "
                        + "contain any questions.");
                }
            }

            foreach (CourseReviewAssignmentViewModel assignment
                in viewModel.AssignmentTemplates)
            {
                if (!assignment.HasRubric)
                {
                    viewModel.ContentWarnings.Add(
                        $"Assignment \"{assignment.Title}\" "
                        + "does not have a rubric.");
                }
            }
        }

        public async Task<ServiceResult> ApproveAsync(
    long courseId,
    long reviewedBy,
    string? reviewComment)
        {
            if (courseId <= 0)
            {
                return ServiceResult.Failure(
                    "Invalid course.");
            }

            if (reviewedBy <= 0)
            {
                return ServiceResult.Failure(
                    "The current Admin account could not be identified.");
            }

            if (!string.IsNullOrWhiteSpace(reviewComment)
                && reviewComment.Trim().Length > 1000)
            {
                return ServiceResult.Failure(
                    "Review comment cannot exceed 1000 characters.");
            }

            Course? course =
                await _context.Courses
                    .FirstOrDefaultAsync(course =>
                        course.CourseId == courseId);

            if (course == null)
            {
                return ServiceResult.Failure(
                    "The course was not found.");
            }

            if (course.Status != CourseStatuses.PendingReview)
            {
                return ServiceResult.Failure(
                    "Only courses currently pending review "
                    + "can be approved.");
            }

            int moduleCount =
                await _context.Modules
                    .CountAsync(module =>
                        module.CourseId == courseId);

            if (moduleCount == 0)
            {
                return ServiceResult.Failure(
                    "The course cannot be approved because "
                    + "it does not contain any modules.");
            }

            int lessonCount =
                await (
                    from lesson in _context.Lessons
                    join module in _context.Modules
                        on lesson.ModuleId equals module.ModuleId
                    where module.CourseId == courseId
                    select lesson.LessonId
                ).CountAsync();

            if (lessonCount == 0)
            {
                return ServiceResult.Failure(
                    "The course cannot be approved because "
                    + "it does not contain any lessons.");
            }

            int draftLessonCount =
                await (
                    from lesson in _context.Lessons
                    join module in _context.Modules
                        on lesson.ModuleId equals module.ModuleId
                    where module.CourseId == courseId
                          && lesson.Status == "DRAFT"
                    select lesson.LessonId
                ).CountAsync();

            if (draftLessonCount > 0)
            {
                return ServiceResult.Failure(
                    $"The course cannot be approved because it "
                    + $"contains {draftLessonCount} draft lesson(s).");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                DateTime reviewedAt = DateTime.UtcNow;

                course.Status =
                    CourseStatuses.Approved;

                course.UpdatedAt =
                    reviewedAt;

                CourseReview review =
                    new CourseReview
                    {
                        CourseId =
                            courseId,

                        ReviewedBy =
                            reviewedBy,

                        Decision =
                            CourseReviewDecisions.Approved,

                        ReviewComment =
                            string.IsNullOrWhiteSpace(reviewComment)
                                ? null
                                : reviewComment.Trim(),

                        ReviewedAt =
                            reviewedAt
                    };

                _context.CourseReviews.Add(review);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return ServiceResult.Success(
                    "The course has been approved successfully.");
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();

                return ServiceResult.Failure(
                    "The course could not be approved because "
                    + "the database update failed.");
            }
            catch
            {
                await transaction.RollbackAsync();

                return ServiceResult.Failure(
                    "An unexpected error occurred while "
                    + "approving the course.");
            }
        }

        public async Task<ServiceResult> RejectAsync(
    long courseId,
    long reviewedBy,
    string? reviewComment)
        {
            if (courseId <= 0)
            {
                return ServiceResult.Failure(
                    "Invalid course.");
            }

            if (reviewedBy <= 0)
            {
                return ServiceResult.Failure(
                    "The current Admin account could not be identified.");
            }

            if (string.IsNullOrWhiteSpace(reviewComment))
            {
                return ServiceResult.Failure(
                    "A rejection reason is required.");
            }

            string normalizedComment =
                reviewComment.Trim();

            if (normalizedComment.Length > 1000)
            {
                return ServiceResult.Failure(
                    "Rejection reason cannot exceed 1000 characters.");
            }

            Course? course =
                await _context.Courses
                    .FirstOrDefaultAsync(course =>
                        course.CourseId == courseId);

            if (course == null)
            {
                return ServiceResult.Failure(
                    "The course was not found.");
            }

            if (course.Status != CourseStatuses.PendingReview)
            {
                return ServiceResult.Failure(
                    "Only courses currently pending review "
                    + "can be rejected.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                DateTime reviewedAt =
                    DateTime.UtcNow;

                course.Status =
                    CourseStatuses.Rejected;

                course.UpdatedAt =
                    reviewedAt;

                CourseReview review =
                    new CourseReview
                    {
                        CourseId =
                            courseId,

                        ReviewedBy =
                            reviewedBy,

                        Decision =
                            CourseReviewDecisions.Rejected,

                        ReviewComment =
                            normalizedComment,

                        ReviewedAt =
                            reviewedAt
                    };

                _context.CourseReviews.Add(review);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return ServiceResult.Success(
                    "The course has been rejected and returned "
                    + "to the SME for revision.");
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();

                return ServiceResult.Failure(
                    "The course could not be rejected because "
                    + "the database update failed.");
            }
            catch
            {
                await transaction.RollbackAsync();

                return ServiceResult.Failure(
                    "An unexpected error occurred while "
                    + "rejecting the course.");
            }
        }
    }
}