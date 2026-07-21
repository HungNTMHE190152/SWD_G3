using EduNexus.Areas.Admin.ViewModels.Dashboard;
using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Services.Administration.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly EduNexusContext _context;

        public AdminDashboardService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            AdminDashboardViewModel viewModel =
                new AdminDashboardViewModel();

            // 1. Tổng số người dùng
            viewModel.TotalUsers =
                await _context.Users
                    .AsNoTracking()
                    .CountAsync();

            // 2. Tài khoản đang hoạt động
            viewModel.ActiveAccounts =
                await _context.Accounts
                    .AsNoTracking()
                    .CountAsync(account => account.IsActive);

            // 3. Số Student
            viewModel.StudentCount =
                await CountAccountsByRoleAsync(RoleNames.Student);

            // 4. Số Teacher
            viewModel.TeacherCount =
                await CountAccountsByRoleAsync(RoleNames.Teacher);

            // 5. Số SME
            viewModel.SmeCount =
                await CountAccountsByRoleAsync(RoleNames.Sme);

            // 6. Course đang chờ duyệt
            viewModel.PendingCourseCount =
                await _context.Courses
                    .AsNoTracking()
                    .CountAsync(course =>
                        course.Status ==
                        CourseStatuses.PendingReview);

            // 7. Classroom đang hoạt động
            viewModel.ActiveClassroomCount =
                await _context.Classrooms
                    .AsNoTracking()
                    .CountAsync(classroom =>
                        classroom.Status ==
                            ClassroomStatuses.Open
                        ||
                        classroom.Status ==
                            ClassroomStatuses.InProgress);

            // 8. Classroom đã hoàn thành
            viewModel.CompletedClassroomCount =
                await _context.Classrooms
                    .AsNoTracking()
                    .CountAsync(classroom =>
                        classroom.Status ==
                        ClassroomStatuses.Completed);

            // 9. Năm Course mới nhất đang chờ duyệt
            viewModel.RecentPendingCourses =
                await GetRecentPendingCoursesAsync();

            return viewModel;
        }

        private async Task<int> CountAccountsByRoleAsync(
            string roleName)
        {
            return await (
                from account in _context.Accounts.AsNoTracking()
                join role in _context.Roles.AsNoTracking()
                    on account.RoleId equals role.RoleId
                where role.RoleName == roleName
                select account.AccountId
            ).CountAsync();
        }

        private async Task<List<PendingCourseItemViewModel>>
            GetRecentPendingCoursesAsync()
        {
            List<PendingCourseItemViewModel> courses =
                await (
                    from course in _context.Courses.AsNoTracking()

                    join category in
                        _context.Categories.AsNoTracking()
                        on course.CategoryId
                        equals category.CategoryId

                    join sme in
                        _context.Users.AsNoTracking()
                        on course.CreatedBy
                        equals sme.UserId

                    where course.Status ==
                        CourseStatuses.PendingReview

                    orderby course.SubmittedAt descending

                    select new PendingCourseItemViewModel
                    {
                        CourseId = course.CourseId,
                        CourseCode = course.CourseCode,
                        Title = course.Title,
                        CategoryName = category.CategoryName,
                        SmeName = sme.FullName,
                        SubmittedAt = course.SubmittedAt,

                        ModuleCount =
                            _context.Modules.Count(module =>
                                module.CourseId ==
                                course.CourseId),

                        LessonCount =
                            (
                                from lesson in _context.Lessons
                                join module in _context.Modules
                                    on lesson.ModuleId
                                    equals module.ModuleId
                                where module.CourseId ==
                                    course.CourseId
                                select lesson.LessonId
                            ).Count()
                    }
                )
                .Take(5)
                .ToListAsync();

            return courses;
        }
    }
}