using EduNexus.Data;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Flashcards;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EduNexus.Services.Implementations;

public class FlashcardService : IFlashcardService
{
    private readonly EduNexusContext _context;
    public FlashcardService(EduNexusContext context)
    {
        _context = context;
    }

    public async Task CreateFlashcardAsync(FlashcardFormViewModel model)
    {
        var flashcard = new Models.Flashcard
        {
            FlashcardSetId = model.FlashcardSetId,
            FrontContent = model.FrontContent,
            BackContent = model.BackContent,
            DisplayOrder = model.DisplayOrder,
            CreatedAt = DateTime.Now
        };

        _context.Flashcards.Add(flashcard);

        await _context.SaveChangesAsync();
    }

    public async Task CreateFlashcardSetAsync(long userId, FlashcardSetFormViewModel model)
    {
        var flashcardSet = new Models.FlashcardSet
        {
            CourseId = model.CourseId,
            CreatedBy = userId,
            SetName = model.SetName,
            Description = model.Description,
            IsAigenerated = model.IsAIGenerated,
            CreatedAt = DateTime.Now
        };

        _context.FlashcardSets.Add(flashcardSet);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFlashcardAsync(long flashcardId)
    {
        var flashcard = await _context.Flashcards
            .FirstOrDefaultAsync(f => f.FlashcardId == flashcardId);

        if (flashcard == null)
        {
            return;
        }

        _context.Flashcards.Remove(flashcard);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFlashcardSetAsync(long flashcardSetId)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(f => f.Flashcards)
            .FirstOrDefaultAsync(f => f.FlashcardSetId == flashcardSetId);

        if (flashcardSet == null)
        {
            return;
        }

        _context.Flashcards.RemoveRange(flashcardSet.Flashcards);
        _context.FlashcardSets.Remove(flashcardSet);
        await _context.SaveChangesAsync();
    }

    public async Task<FlashcardSetFormViewModel> GetCreateSetViewModelAsync()
    {
        var model = new FlashcardSetFormViewModel();

        model.Courses = await _context.Courses
            .OrderBy(c => c.Title)
            .Select(c => new CourseOptionViewModel
            {
                CourseId = c.CourseId,
                CourseTitle = c.Title
            })
            .ToListAsync();

        return model;
    }

    public async Task<FlashcardSetFormViewModel?> GetEditSetViewModelAsync(long flashcardSetId)
    {
        var flashcardSet = await _context.FlashcardSets
            .FirstOrDefaultAsync(f => f.FlashcardSetId == flashcardSetId);

        if (flashcardSet == null)
        {
            return null;
        }

        var model = new FlashcardSetFormViewModel
        {
            FlashcardSetId = flashcardSet.FlashcardSetId,
            CourseId = flashcardSet.CourseId,
            SetName = flashcardSet.SetName,
            Description = flashcardSet.Description,
            IsAIGenerated = flashcardSet.IsAigenerated ?? false
        };

        model.Courses = await _context.Courses
            .OrderBy(c => c.Title)
            .Select(c => new CourseOptionViewModel
            {
                CourseId = c.CourseId,
                CourseTitle = c.Title
            })
            .ToListAsync();

        return model;
    }

    public async Task<FlashcardEditorViewModel?> GetFlashcardEditorAsync(long flashcardSetId)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(f => f.Flashcards)
            .Include(f => f.Course)
            .FirstOrDefaultAsync(f => f.FlashcardSetId == flashcardSetId);

        if (flashcardSet == null)
        {
            return null;
        }

        return new FlashcardEditorViewModel
        {
            FlashcardSetId = flashcardSet.FlashcardSetId,

            SetName = flashcardSet.SetName,

            Description = flashcardSet.Description,

            CourseTitle = flashcardSet.Course.Title,

            Flashcards = flashcardSet.Flashcards
                .OrderBy(f => f.DisplayOrder)
                .Select(f => new FlashcardItemViewModel
                {
                    FlashcardId = f.FlashcardId,

                    FrontContent = f.FrontContent,

                    BackContent = f.BackContent,

                    DisplayOrder = f.DisplayOrder ?? 0
                })
                .ToList()
        };
    }
    public async Task<FlashcardSetListViewModel> GetMyFlashcardSetsAsync(long userId)
    {
        var flashcardSets = await _context.FlashcardSets
            .Include(x => x.Course)
            .Include(x => x.Flashcards)
            .Where(x => x.CreatedBy == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var model = new FlashcardSetListViewModel();

        model.FlashcardSets = flashcardSets.Select(x => new FlashcardSetItemViewModel
        {
            FlashcardSetId = x.FlashcardSetId,
            SetName = x.SetName,
            Description = x.Description,
            CourseTitle = x.Course.Title,
            TotalCards = x.Flashcards.Count,
            IsAIGenerated = x.IsAigenerated ?? false,
            CreatedAt = x.CreatedAt
        }).ToList();

        return model;
    }
    public async Task UpdateFlashcardAsync(FlashcardFormViewModel model)
    {
        var flashcard = await _context.Flashcards
            .FirstOrDefaultAsync(f => f.FlashcardId == model.FlashcardId);

        if (flashcard == null)
        {
            return;
        }

        flashcard.FrontContent = model.FrontContent;
        flashcard.BackContent = model.BackContent;
        flashcard.DisplayOrder = model.DisplayOrder;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateFlashcardSetAsync(long userId, FlashcardSetFormViewModel model)
    {
        var flashcardSet = await _context.FlashcardSets
            .FirstOrDefaultAsync(f => f.FlashcardSetId == model.FlashcardSetId);

        if (flashcardSet == null)
        {
            return;
        }

        flashcardSet.CourseId = model.CourseId;
        flashcardSet.SetName = model.SetName;
        flashcardSet.Description = model.Description;
        flashcardSet.IsAigenerated = model.IsAIGenerated;

        await _context.SaveChangesAsync();
    }
    public async Task<FlashcardStagingViewModel?> GetFlashcardStagingAsync(long flashcardSetId)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Course)
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x => x.FlashcardSetId == flashcardSetId);

        if (flashcardSet == null)
        {
            return null;
        }

        var model = new FlashcardStagingViewModel
        {
            FlashcardSetId = flashcardSet.FlashcardSetId,

            SetName = flashcardSet.SetName,

            CourseTitle = flashcardSet.Course.Title,

            Description = flashcardSet.Description,

            TotalCards = flashcardSet.Flashcards.Count,

            Flashcards = flashcardSet.Flashcards
        .OrderBy(f => f.DisplayOrder)
        .Select(f => new FlashcardItemViewModel
        {
            FlashcardId = f.FlashcardId,
            FrontContent = f.FrontContent,
            BackContent = f.BackContent,
            DisplayOrder = f.DisplayOrder ?? 0
        })
        .ToList()
        };
        var validations = new List<FlashcardValidationItemViewModel>();
        validations.Add(new FlashcardValidationItemViewModel
        {
            RuleName = "Flashcard Set contains cards",

            IsPassed = model.TotalCards > 0,

            Message = model.TotalCards > 0
        ? $"{model.TotalCards} flashcards found."
        : "Flashcard Set has no flashcards."
        });
        bool hasEmptyQuestion = model.Flashcards.Any(x =>
        string.IsNullOrWhiteSpace(x.FrontContent));

        validations.Add(new FlashcardValidationItemViewModel
        {
            RuleName = "All questions completed",

            IsPassed = !hasEmptyQuestion,

            Message = hasEmptyQuestion
                ? "Some flashcards have empty questions."
                : "All questions are completed."
        });
        bool hasEmptyAnswer = model.Flashcards.Any(x =>
        string.IsNullOrWhiteSpace(x.BackContent));

        validations.Add(new FlashcardValidationItemViewModel
        {
            RuleName = "All answers completed",

            IsPassed = !hasEmptyAnswer,

            Message = hasEmptyAnswer
                ? "Some flashcards have empty answers."
                : "All answers are completed."
        });
        bool duplicateOrder = model.Flashcards
            .GroupBy(x => x.DisplayOrder)
            .Any(g => g.Count() > 1);

        validations.Add(new FlashcardValidationItemViewModel
        {
            RuleName = "Display Order is unique",

            IsPassed = !duplicateOrder,

            Message = duplicateOrder
                ? "Duplicate display orders found."
                : "Display orders are unique."
        });
        model.ValidationResults = validations;

        model.TotalRules = validations.Count;

        model.PassedRules = validations.Count(x => x.IsPassed);
        model.AverageQuestionLength =
        model.Flashcards.Any()
        ? (int)model.Flashcards.Average(x => x.FrontContent.Length)
        : 0;

        model.AverageAnswerLength =
            model.Flashcards.Any()
                ? (int)model.Flashcards.Average(x => x.BackContent.Length)
                : 0;

        model.MaxDisplayOrder =
            model.Flashcards.Any()
                ? model.Flashcards.Max(x => x.DisplayOrder)
                : 0;

        model.EmptyQuestionCount =
            model.Flashcards.Count(x =>
                string.IsNullOrWhiteSpace(x.FrontContent));

        model.EmptyAnswerCount =
            model.Flashcards.Count(x =>
                string.IsNullOrWhiteSpace(x.BackContent));
        return model;
    }
    public async Task<FlashcardPreviewViewModel?> GetFlashcardPreviewAsync(long flashcardSetId)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x => x.FlashcardSetId == flashcardSetId);

        if (flashcardSet == null)
            return null;

        return new FlashcardPreviewViewModel
        {
            FlashcardSetId = flashcardSet.FlashcardSetId,

            SetName = flashcardSet.SetName,

            Flashcards = flashcardSet.Flashcards
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new FlashcardItemViewModel
                {
                    FlashcardId = x.FlashcardId,

                    FrontContent = x.FrontContent,

                    BackContent = x.BackContent,

                    DisplayOrder = x.DisplayOrder ?? 0
                })
                .ToList()
        };
    }
    public async Task<FlashcardPracticeViewModel?> GetFlashcardPracticeAsync(long flashcardSetId)
    {
        var flashcardSet = await _context.FlashcardSets
            .Include(x => x.Flashcards)
            .FirstOrDefaultAsync(x => x.FlashcardSetId == flashcardSetId);

        if (flashcardSet == null)
        {
            return null;
        }

        return new FlashcardPracticeViewModel
        {
            FlashcardSetId = flashcardSet.FlashcardSetId,

            SetName = flashcardSet.SetName,

            TotalCards = flashcardSet.Flashcards.Count,

            Flashcards = flashcardSet.Flashcards
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new FlashcardPracticeItemViewModel
                {
                    FlashcardId = x.FlashcardId,

                    FrontContent = x.FrontContent,

                    BackContent = x.BackContent,

                    DisplayOrder = x.DisplayOrder ?? 0
                })
                .ToList()
        };
    }
    public async Task<FlashcardLibraryViewModel> GetFlashcardLibraryAsync(string? keyword,long? courseId,string? sortBy)
{
        var query = _context.FlashcardSets
        .Include(x => x.Course)
        .Include(x => x.Flashcards)
            .ThenInclude(x => x.FlashcardPractices)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                x.SetName.Contains(keyword) ||
                (x.Description != null && x.Description.Contains(keyword)) ||
                x.Course.Title.Contains(keyword));
        }

        if (courseId.HasValue)
        {
            query = query.Where(x => x.CourseId == courseId.Value);
        }

        query = sortBy switch
        {
            "oldest" => query.OrderBy(x => x.CreatedAt),

            "name" => query.OrderBy(x => x.SetName),

            "cards" => query.OrderByDescending(x => x.Flashcards.Count),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var flashcardSets = await query.ToListAsync();

        var model = new FlashcardLibraryViewModel
        {
            Keyword = keyword,
            CourseId = courseId,
            SortBy = sortBy ?? "newest"
        };

        model.FlashcardSets = flashcardSets.Select(x => new FlashcardLibraryItemViewModel
    {
        FlashcardSetId = x.FlashcardSetId,
        SetName = x.SetName,
        CourseTitle = x.Course.Title,
        Description = x.Description,
        TotalCards = x.Flashcards.Count,
        CreatedAt = x.CreatedAt
    }).ToList();

        model.FlashcardSets = flashcardSets.Select(set =>
        {
            var practices = set.Flashcards
                .SelectMany(f => f.FlashcardPractices)
                .ToList();

            var totalReviews = practices.Sum(x => x.ReviewCount ?? 0);

            var totalCorrect = practices.Sum(x => x.CorrectCount ?? 0);

            var accuracy = totalReviews == 0
                ? 0
                : (double)totalCorrect / totalReviews * 100;

            return new FlashcardLibraryItemViewModel
            {
                FlashcardSetId = set.FlashcardSetId,
                SetName = set.SetName,
                CourseTitle = set.Course.Title,
                Description = set.Description,
                TotalCards = set.Flashcards.Count,
                CreatedAt = set.CreatedAt,

                HasPracticed = practices.Any(),

                LastReviewed = practices
                    .Max(x => x.LastReviewed),

                TotalReviews = totalReviews,

                TotalCorrect = totalCorrect,

                Accuracy = Math.Round(accuracy, 1)
            };

        }).ToList();

        model.TotalSets = model.FlashcardSets.Count;

    model.TotalFlashcards = model.FlashcardSets.Sum(x => x.TotalCards);

    model.TotalCourses = model.FlashcardSets
        .Select(x => x.CourseTitle)
        .Distinct()
        .Count();
    model.Courses = await _context.Courses
        .OrderBy(x => x.Title)
        .Select(x => new CourseFilterItemViewModel
    {
        CourseId = x.CourseId,
        CourseTitle = x.Title
    })
    .ToListAsync();

        return model;
}

    public async Task SavePracticeAsync(
    long studentId,
    FlashcardPracticeSubmitViewModel model)
    {
        foreach (var answer in model.Answers)
        {
            var practice = await _context.FlashcardPractices
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.FlashcardId == answer.FlashcardId);

            if (practice == null)
            {
                practice = new Models.FlashcardPractice
                {
                    StudentId = studentId,
                    FlashcardId = answer.FlashcardId,
                    ReviewCount = 1,
                    CorrectCount = answer.IsCorrect ? 1 : 0,
                    LastReviewed = DateTime.Now,
                    NextReview = DateTime.Now.AddDays(1)
                };

                _context.FlashcardPractices.Add(practice);
            }
            else
            {
                practice.ReviewCount =
                    (practice.ReviewCount ?? 0) + 1;

                if (answer.IsCorrect)
                {
                    practice.CorrectCount =
                        (practice.CorrectCount ?? 0) + 1;
                }

                practice.LastReviewed = DateTime.Now;

                practice.NextReview = DateTime.Now.AddDays(1);
            }
        }

        await _context.SaveChangesAsync();
    }
}