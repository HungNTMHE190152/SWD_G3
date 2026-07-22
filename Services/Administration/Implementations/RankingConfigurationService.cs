using EduNexus.Areas.Admin.ViewModels.RankingConfiguration;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class RankingConfigurationService
        : IRankingConfigurationService
    {
        private readonly EduNexusContext _context;

        public RankingConfigurationService(
            EduNexusContext context)
        {
            _context = context;
        }

        public async Task<RankingConfigurationIndexViewModel>
            GetIndexAsync()
        {
            RankingConfigurationHistoryItemViewModel?
                activeConfiguration =
                    await (
                        from configuration in
                            _context.RankingConfigurations
                                .AsNoTracking()

                        join updater in
                            _context.Users.AsNoTracking()
                            on configuration.UpdatedBy
                            equals updater.UserId

                        where configuration.IsActive

                        orderby configuration.UpdatedAt descending

                        select new
                            RankingConfigurationHistoryItemViewModel
                        {
                            RankingConfigurationId =
                                    configuration
                                        .RankingConfigurationId,

                            AssignmentPercentage =
                                    configuration
                                        .AssignmentWeight * 100m,

                            QuizPercentage =
                                    configuration
                                        .QuizWeight * 100m,

                            LessonProgressPercentage =
                                    configuration
                                        .LessonProgressWeight * 100m,

                            IsActive =
                                    configuration.IsActive,

                            UpdatedByName =
                                    updater.FullName,

                            UpdatedAt =
                                    configuration.UpdatedAt
                        }
                    ).FirstOrDefaultAsync();

            List<RankingConfigurationHistoryItemViewModel>
                history =
                    await (
                        from configuration in
                            _context.RankingConfigurations
                                .AsNoTracking()

                        join updater in
                            _context.Users.AsNoTracking()
                            on configuration.UpdatedBy
                            equals updater.UserId

                        orderby configuration.UpdatedAt descending

                        select new
                            RankingConfigurationHistoryItemViewModel
                        {
                            RankingConfigurationId =
                                    configuration
                                        .RankingConfigurationId,

                            AssignmentPercentage =
                                    configuration
                                        .AssignmentWeight * 100m,

                            QuizPercentage =
                                    configuration
                                        .QuizWeight * 100m,

                            LessonProgressPercentage =
                                    configuration
                                        .LessonProgressWeight * 100m,

                            IsActive =
                                    configuration.IsActive,

                            UpdatedByName =
                                    updater.FullName,

                            UpdatedAt =
                                    configuration.UpdatedAt
                        }
                    )
                    .Take(20)
                    .ToListAsync();

            RankingConfigurationFormViewModel form;

            if (activeConfiguration == null)
            {
                form = new RankingConfigurationFormViewModel
                {
                    AssignmentPercentage = 50m,
                    QuizPercentage = 30m,
                    LessonProgressPercentage = 20m
                };
            }
            else
            {
                form = new RankingConfigurationFormViewModel
                {
                    AssignmentPercentage =
                        activeConfiguration
                            .AssignmentPercentage,

                    QuizPercentage =
                        activeConfiguration
                            .QuizPercentage,

                    LessonProgressPercentage =
                        activeConfiguration
                            .LessonProgressPercentage
                };
            }

            return new RankingConfigurationIndexViewModel
            {
                Form = form,

                HasActiveConfiguration =
                    activeConfiguration != null,

                ActiveConfigurationId =
                    activeConfiguration
                        ?.RankingConfigurationId,

                CurrentUpdatedByName =
                    activeConfiguration
                        ?.UpdatedByName,

                CurrentUpdatedAt =
                    activeConfiguration
                        ?.UpdatedAt,

                History = history
            };
        }

        public async Task<ServiceResult>
            SaveConfigurationAsync(
                RankingConfigurationFormViewModel model,
                long updatedBy)
        {
            if (updatedBy <= 0)
            {
                return ServiceResult.Failure(
                    "The current Admin account "
                    + "could not be identified.");
            }

            bool updaterExists =
                await _context.Users
                    .AsNoTracking()
                    .AnyAsync(user =>
                        user.UserId == updatedBy);

            if (!updaterExists)
            {
                return ServiceResult.Failure(
                    "The current Admin user was not found.");
            }

            if (model.AssignmentPercentage < 0m
                || model.AssignmentPercentage > 100m)
            {
                return ServiceResult.Failure(
                    "Assignment weight must be "
                    + "between 0 and 100.");
            }

            if (model.QuizPercentage < 0m
                || model.QuizPercentage > 100m)
            {
                return ServiceResult.Failure(
                    "Quiz weight must be "
                    + "between 0 and 100.");
            }

            if (model.LessonProgressPercentage < 0m
                || model.LessonProgressPercentage > 100m)
            {
                return ServiceResult.Failure(
                    "Lesson progress weight must be "
                    + "between 0 and 100.");
            }

            decimal totalPercentage =
                model.AssignmentPercentage
                + model.QuizPercentage
                + model.LessonProgressPercentage;

            if (totalPercentage != 100m)
            {
                return ServiceResult.Failure(
                    $"The total weight must equal 100%. "
                    + $"Current total: {totalPercentage}%.");
            }

            decimal assignmentWeight =
                decimal.Round(
                    model.AssignmentPercentage / 100m,
                    5,
                    MidpointRounding.AwayFromZero);

            decimal quizWeight =
                decimal.Round(
                    model.QuizPercentage / 100m,
                    5,
                    MidpointRounding.AwayFromZero);

            decimal lessonProgressWeight =
                decimal.Round(
                    model.LessonProgressPercentage / 100m,
                    5,
                    MidpointRounding.AwayFromZero);

            decimal totalDatabaseWeight =
                assignmentWeight
                + quizWeight
                + lessonProgressWeight;

            if (totalDatabaseWeight != 1.00000m)
            {
                return ServiceResult.Failure(
                    "The converted weights do not equal 1.00000. "
                    + "Please use values with fewer decimal places.");
            }

            RankingConfiguration? currentConfiguration =
                await _context.RankingConfigurations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(configuration =>
                        configuration.IsActive);

            if (currentConfiguration != null
                && currentConfiguration.AssignmentWeight
                    == assignmentWeight
                && currentConfiguration.QuizWeight
                    == quizWeight
                && currentConfiguration.LessonProgressWeight
                    == lessonProgressWeight)
            {
                return ServiceResult.Failure(
                    "These weights are already active. "
                    + "No changes were saved.");
            }

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                List<RankingConfiguration>
                    activeConfigurations =
                        await _context
                            .RankingConfigurations
                            .Where(configuration =>
                                configuration.IsActive)
                            .ToListAsync();

                foreach (RankingConfiguration configuration
                    in activeConfigurations)
                {
                    configuration.IsActive = false;
                }

                /*
                 * Save deactivation first so the unique
                 * filtered index permits inserting the
                 * new active configuration.
                 */
                await _context.SaveChangesAsync();

                RankingConfiguration newConfiguration =
                    new RankingConfiguration
                    {
                        AssignmentWeight =
                            assignmentWeight,

                        QuizWeight =
                            quizWeight,

                        LessonProgressWeight =
                            lessonProgressWeight,

                        IsActive = true,

                        UpdatedBy =
                            updatedBy,

                        UpdatedAt =
                            DateTime.UtcNow
                    };

                _context.RankingConfigurations.Add(
                    newConfiguration);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return ServiceResult.Success(
                    "The student ranking configuration "
                    + "was updated successfully.");
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();

                return ServiceResult.Failure(
                    "The configuration could not be saved "
                    + "because the database rejected the values.");
            }
            catch
            {
                await transaction.RollbackAsync();

                return ServiceResult.Failure(
                    "An unexpected error occurred while "
                    + "saving the ranking configuration.");
            }
        }
    }
}