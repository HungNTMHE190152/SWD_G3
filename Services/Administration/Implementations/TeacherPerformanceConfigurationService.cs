using EduNexus.Areas.Admin.ViewModels
    .TeacherPerformanceConfiguration;
using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Administration.Interfaces;
using EduNexus.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class TeacherPerformanceConfigurationService
        : ITeacherPerformanceConfigurationService
    {
        private readonly EduNexusContext _context;

        public TeacherPerformanceConfigurationService(
            EduNexusContext context)
        {
            _context = context;
        }

        public async Task<
            TeacherPerformanceConfigurationIndexViewModel>
            GetIndexAsync()
        {
            TeacherPerformanceConfigurationHistoryItemViewModel?
                activeConfiguration =
                    await (
                        from configuration in
                            _context
                                .TeacherPerformanceConfigurations
                                .AsNoTracking()

                        join updater in
                            _context.Users.AsNoTracking()
                            on configuration.UpdatedBy
                            equals updater.UserId

                        where configuration.IsActive

                        orderby configuration.UpdatedAt descending

                        select new
                            TeacherPerformanceConfigurationHistoryItemViewModel
                        {
                            TeacherPerformanceConfigurationId =
                                    configuration
                                        .TeacherPerformanceConfigurationId,

                            FeedbackPercentage =
                                    configuration.FeedbackWeight
                                    * 100m,

                            CompletionPercentage =
                                    configuration.CompletionWeight
                                    * 100m,

                            StudentPerformancePercentage =
                                    configuration
                                        .StudentPerformanceWeight
                                    * 100m,

                            MinimumEvaluationCount =
                                    configuration
                                        .MinimumEvaluationCount,

                            IsActive =
                                    configuration.IsActive,

                            UpdatedByName =
                                    updater.FullName,

                            UpdatedAt =
                                    configuration.UpdatedAt
                        }
                    ).FirstOrDefaultAsync();

            List<
                TeacherPerformanceConfigurationHistoryItemViewModel>
                history =
                    await (
                        from configuration in
                            _context
                                .TeacherPerformanceConfigurations
                                .AsNoTracking()

                        join updater in
                            _context.Users.AsNoTracking()
                            on configuration.UpdatedBy
                            equals updater.UserId

                        orderby configuration.UpdatedAt descending

                        select new
                            TeacherPerformanceConfigurationHistoryItemViewModel
                        {
                            TeacherPerformanceConfigurationId =
                                    configuration
                                        .TeacherPerformanceConfigurationId,

                            FeedbackPercentage =
                                    configuration.FeedbackWeight
                                    * 100m,

                            CompletionPercentage =
                                    configuration.CompletionWeight
                                    * 100m,

                            StudentPerformancePercentage =
                                    configuration
                                        .StudentPerformanceWeight
                                    * 100m,

                            MinimumEvaluationCount =
                                    configuration
                                        .MinimumEvaluationCount,

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

            TeacherPerformanceConfigurationFormViewModel form;

            if (activeConfiguration == null)
            {
                form =
                    new TeacherPerformanceConfigurationFormViewModel
                    {
                        FeedbackPercentage = 60m,
                        CompletionPercentage = 20m,
                        StudentPerformancePercentage = 20m,
                        MinimumEvaluationCount = 5
                    };
            }
            else
            {
                form =
                    new TeacherPerformanceConfigurationFormViewModel
                    {
                        FeedbackPercentage =
                            activeConfiguration
                                .FeedbackPercentage,

                        CompletionPercentage =
                            activeConfiguration
                                .CompletionPercentage,

                        StudentPerformancePercentage =
                            activeConfiguration
                                .StudentPerformancePercentage,

                        MinimumEvaluationCount =
                            activeConfiguration
                                .MinimumEvaluationCount
                    };
            }

            return new
                TeacherPerformanceConfigurationIndexViewModel
            {
                Form = form,

                HasActiveConfiguration =
                        activeConfiguration != null,

                ActiveConfigurationId =
                        activeConfiguration
                            ?.TeacherPerformanceConfigurationId,

                CurrentFeedbackPercentage =
                        activeConfiguration
                            ?.FeedbackPercentage,

                CurrentCompletionPercentage =
                        activeConfiguration
                            ?.CompletionPercentage,

                CurrentStudentPerformancePercentage =
                        activeConfiguration
                            ?.StudentPerformancePercentage,

                CurrentMinimumEvaluationCount =
                        activeConfiguration
                            ?.MinimumEvaluationCount,

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
                TeacherPerformanceConfigurationFormViewModel model,
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

            if (model.FeedbackPercentage < 0m
                || model.FeedbackPercentage > 100m)
            {
                return ServiceResult.Failure(
                    "Student feedback weight must "
                    + "be between 0 and 100.");
            }

            if (model.CompletionPercentage < 0m
                || model.CompletionPercentage > 100m)
            {
                return ServiceResult.Failure(
                    "Classroom completion weight must "
                    + "be between 0 and 100.");
            }

            if (model.StudentPerformancePercentage < 0m
                || model.StudentPerformancePercentage > 100m)
            {
                return ServiceResult.Failure(
                    "Student performance weight must "
                    + "be between 0 and 100.");
            }

            if (model.MinimumEvaluationCount <= 0)
            {
                return ServiceResult.Failure(
                    "Minimum evaluation count must "
                    + "be greater than zero.");
            }

            decimal totalPercentage =
                model.FeedbackPercentage
                + model.CompletionPercentage
                + model.StudentPerformancePercentage;

            if (totalPercentage != 100m)
            {
                return ServiceResult.Failure(
                    $"The total weight must equal 100%. "
                    + $"Current total: {totalPercentage}%.");
            }

            decimal feedbackWeight =
                decimal.Round(
                    model.FeedbackPercentage / 100m,
                    5,
                    MidpointRounding.AwayFromZero);

            decimal completionWeight =
                decimal.Round(
                    model.CompletionPercentage / 100m,
                    5,
                    MidpointRounding.AwayFromZero);

            decimal studentPerformanceWeight =
                decimal.Round(
                    model.StudentPerformancePercentage / 100m,
                    5,
                    MidpointRounding.AwayFromZero);

            decimal totalDatabaseWeight =
                feedbackWeight
                + completionWeight
                + studentPerformanceWeight;

            if (totalDatabaseWeight != 1.00000m)
            {
                return ServiceResult.Failure(
                    "The converted weights do not equal "
                    + "1.00000. Please use values with "
                    + "fewer decimal places.");
            }

            TeacherPerformanceConfiguration?
                currentConfiguration =
                    await _context
                        .TeacherPerformanceConfigurations
                        .AsNoTracking()
                        .FirstOrDefaultAsync(configuration =>
                            configuration.IsActive);

            if (currentConfiguration != null
                && currentConfiguration.FeedbackWeight
                    == feedbackWeight
                && currentConfiguration.CompletionWeight
                    == completionWeight
                && currentConfiguration.StudentPerformanceWeight
                    == studentPerformanceWeight
                && currentConfiguration.MinimumEvaluationCount
                    == model.MinimumEvaluationCount)
            {
                return ServiceResult.Failure(
                    "These teacher performance settings "
                    + "are already active. No changes were saved.");
            }

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();

            try
            {
                List<TeacherPerformanceConfiguration>
                    activeConfigurations =
                        await _context
                            .TeacherPerformanceConfigurations
                            .Where(configuration =>
                                configuration.IsActive)
                            .ToListAsync();

                foreach (
                    TeacherPerformanceConfiguration configuration
                    in activeConfigurations)
                {
                    configuration.IsActive = false;
                }

                /*
                 * Save the deactivation first because the
                 * database permits only one active record.
                 */
                await _context.SaveChangesAsync();

                TeacherPerformanceConfiguration
                    newConfiguration =
                        new TeacherPerformanceConfiguration
                        {
                            FeedbackWeight =
                                feedbackWeight,

                            CompletionWeight =
                                completionWeight,

                            StudentPerformanceWeight =
                                studentPerformanceWeight,

                            MinimumEvaluationCount =
                                model.MinimumEvaluationCount,

                            IsActive = true,

                            UpdatedBy =
                                updatedBy,

                            UpdatedAt =
                                DateTime.UtcNow
                        };

                _context
                    .TeacherPerformanceConfigurations
                    .Add(newConfiguration);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return ServiceResult.Success(
                    "The teacher performance configuration "
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
                    + "saving the teacher performance configuration.");
            }
        }
    }
}