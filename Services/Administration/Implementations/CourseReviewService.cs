using EduNexus.Areas.Admin.ViewModels.CourseReview;
using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Administration.Interfaces;
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
    }
}