using EduNexus.Areas.Admin.ViewModels.Category;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly EduNexusContext _context;

        public CategoryService(EduNexusContext context)
        {
            _context = context;
        }

        public async Task<CategoryIndexViewModel> GetCategoriesAsync(
            CategoryFilterViewModel filter)
        {
            filter.Page = filter.Page < 1
                ? 1
                : filter.Page;

            filter.PageSize =
                filter.PageSize is < 5 or > 100
                    ? 10
                    : filter.PageSize;

            IQueryable<Category> query =
                _context.Categories
                    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                string keyword = filter.SearchText.Trim();

                query = query.Where(category =>
                    category.CategoryName.Contains(keyword)
                    || (
                        category.Description != null
                        && category.Description.Contains(keyword)
                    ));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(category =>
                    category.IsActive == filter.IsActive.Value);
            }

            int totalItems = await query.CountAsync();

            int totalPages = totalItems == 0
                ? 1
                : (int)Math.Ceiling(
                    totalItems / (double)filter.PageSize);

            if (filter.Page > totalPages)
            {
                filter.Page = totalPages;
            }

            List<CategoryListItemViewModel> categories =
                await query
                    .OrderBy(category => category.CategoryName)
                    .Skip(
                        (filter.Page - 1)
                        * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(category =>
                        new CategoryListItemViewModel
                        {
                            CategoryId =
                                category.CategoryId,

                            CategoryName =
                                category.CategoryName,

                            Description =
                                category.Description,

                            IsActive =
                                category.IsActive,

                            CreatedAt =
                                category.CreatedAt,

                            CourseCount =
                                _context.Courses.Count(course =>
                                    course.CategoryId
                                    == category.CategoryId)
                        })
                    .ToListAsync();

            return new CategoryIndexViewModel
            {
                Filter = filter,
                Categories = categories,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<CategoryFormViewModel?> GetForEditAsync(
            long categoryId)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(category =>
                    category.CategoryId == categoryId)
                .Select(category =>
                    new CategoryFormViewModel
                    {
                        CategoryId =
                            category.CategoryId,

                        CategoryName =
                            category.CategoryName,

                        Description =
                            category.Description,

                        IsActive =
                            category.IsActive
                    })
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceResult<long>> CreateAsync(
            CategoryFormViewModel model)
        {
            string categoryName =
                model.CategoryName.Trim();

            bool nameExists =
                await _context.Categories
                    .AnyAsync(category =>
                        category.CategoryName == categoryName);

            if (nameExists)
            {
                return ServiceResult<long>.Failure(
                    "A category with this name already exists.");
            }

            Category category = new Category
            {
                CategoryName = categoryName,

                Description =
                    string.IsNullOrWhiteSpace(model.Description)
                        ? null
                        : model.Description.Trim(),

                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return ServiceResult<long>.Success(
                category.CategoryId,
                "The category was created successfully.");
        }

        public async Task<ServiceResult> UpdateAsync(
            CategoryFormViewModel model)
        {
            Category? category =
                await _context.Categories
                    .FirstOrDefaultAsync(category =>
                        category.CategoryId
                        == model.CategoryId);

            if (category == null)
            {
                return ServiceResult.Failure(
                    "The category was not found.");
            }

            string categoryName =
                model.CategoryName.Trim();

            bool nameExists =
                await _context.Categories
                    .AnyAsync(item =>
                        item.CategoryName == categoryName
                        && item.CategoryId != model.CategoryId);

            if (nameExists)
            {
                return ServiceResult.Failure(
                    "A category with this name already exists.");
            }

            category.CategoryName =
                categoryName;

            category.Description =
                string.IsNullOrWhiteSpace(model.Description)
                    ? null
                    : model.Description.Trim();

            category.IsActive =
                model.IsActive;

            await _context.SaveChangesAsync();

            return ServiceResult.Success(
                "The category was updated successfully.");
        }

        public async Task<ServiceResult> SetActiveAsync(
            long categoryId,
            bool isActive)
        {
            Category? category =
                await _context.Categories
                    .FirstOrDefaultAsync(category =>
                        category.CategoryId == categoryId);

            if (category == null)
            {
                return ServiceResult.Failure(
                    "The category was not found.");
            }

            if (category.IsActive == isActive)
            {
                return ServiceResult.Failure(
                    isActive
                        ? "This category is already active."
                        : "This category is already inactive.");
            }

            category.IsActive = isActive;

            await _context.SaveChangesAsync();

            return ServiceResult.Success(
                isActive
                    ? "The category has been activated."
                    : "The category has been deactivated.");
        }
    }
}