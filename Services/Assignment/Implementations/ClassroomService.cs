using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Assignment.Interfaces;
using EduNexus.ViewModels.Assignment;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Assignment.Implementations
{
    public class ClassroomService : IClassroomService
    {
        private readonly EduNexusContext _context;

        public ClassroomService(EduNexusContext context)
        {
            _context = context;
        }

        private async Task<List<CourseOptionViewModel>> GetApprovedCoursesAsync(long teacherId)
        {
            return await _context.Courses
                .Where(x => x.CreatedBy == teacherId)
                .OrderBy(x => x.Title)
                .Select(x => new CourseOptionViewModel
                {
                    CourseId = x.CourseId,
                    CourseCode = x.CourseCode,
                    CourseTitle = x.Title
                })
                .ToListAsync();
        }

        public async Task<List<ClassroomViewModel>> GetAllAsync(long teacherId)
        {
            return await _context.Classrooms
                .Where(x => x.TeacherId == teacherId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ClassroomViewModel
                {
                    ClassroomId = x.ClassroomId,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Title,
                    ClassName = x.ClassName,
                    EnrollmentCode = x.EnrollmentCode,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status
                })
                .ToListAsync();
        }

        public async Task<ClassroomViewModel> GetCreateModelAsync(long teacherId)
        {
            return new ClassroomViewModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3),
                Status = "OPEN",
                Courses = await GetApprovedCoursesAsync(teacherId)
            };
        }

        public async Task<ClassroomViewModel?> GetByIdAsync(long id, long teacherId)
        {
            var entity = await _context.Classrooms
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x =>
                    x.ClassroomId == id &&
                    x.TeacherId == teacherId);

            if (entity == null)
                return null;

            return new ClassroomViewModel
            {
                ClassroomId = entity.ClassroomId,
                CourseId = entity.CourseId,
                CourseName = entity.Course.Title,

                ClassName = entity.ClassName,
                EnrollmentCode = entity.EnrollmentCode,

                StartDate = entity.StartDate,
                EndDate = entity.EndDate,

                Status = entity.Status,

                Courses = await GetApprovedCoursesAsync(teacherId)
            };
        }

        public async Task<bool> CreateAsync(ClassroomViewModel model, long teacherId)
        {
            if (model.StartDate >= model.EndDate)
                return false;

            var existed = await _context.Classrooms
                .AnyAsync(x => x.EnrollmentCode == model.EnrollmentCode);

            if (existed)
                return false;

            var entity = new Classroom
            {
                CourseId = model.CourseId,
                TeacherId = teacherId,
                ClassName = model.ClassName,
                EnrollmentCode = model.EnrollmentCode,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = "OPEN",
                CreatedAt = DateTime.Now
            };

            _context.Classrooms.Add(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(ClassroomViewModel model, long teacherId)
        {
            var entity = await _context.Classrooms
                .FirstOrDefaultAsync(x =>
                    x.ClassroomId == model.ClassroomId &&
                    x.TeacherId == teacherId);

            if (entity == null)
                return false;

            if (model.StartDate >= model.EndDate)
                return false;

            var duplicateCode = await _context.Classrooms.AnyAsync(x =>
                x.EnrollmentCode == model.EnrollmentCode &&
                x.ClassroomId != model.ClassroomId);

            if (duplicateCode)
                return false;

            entity.CourseId = model.CourseId;
            entity.ClassName = model.ClassName;
            entity.EnrollmentCode = model.EnrollmentCode;
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.Status = model.Status;
            entity.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(long id, long teacherId)
        {
            var entity = await _context.Classrooms
                .FirstOrDefaultAsync(x =>
                    x.ClassroomId == id &&
                    x.TeacherId == teacherId);

            if (entity == null)
                return false;

            _context.Classrooms.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}