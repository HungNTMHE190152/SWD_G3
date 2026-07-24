using EduNexus.Areas.Admin.ViewModels.TeacherPerformanceReport;
using EduNexus.Constants;
using EduNexus.Data;
using EduNexus.Services.Administration.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNexus.Services.Administration.Implementations
{
    public class TeacherPerformanceReportService
        : ITeacherPerformanceReportService
    {
        private readonly EduNexusContext _context;

        public TeacherPerformanceReportService(
            EduNexusContext context)
        {
            _context = context;
        }

        /*
         * Kiểu dữ liệu nội bộ dùng để tính toán Classroom.
         * Không cần tạo file riêng.
         */
        private sealed class ClassroomMetricRow
        {
            public long ClassroomId { get; set; }

            public long TeacherId { get; set; }

            public long CourseId { get; set; }

            public string Status { get; set; } =
                string.Empty;
        }

        /*
         * Kiểu dữ liệu nội bộ dùng để tính toán Enrollment.
         * Không cần tạo file riêng.
         */
        private sealed class EnrollmentMetricRow
        {
            public long EnrollmentId { get; set; }

            public long ClassroomId { get; set; }

            public string Status { get; set; } =
                string.Empty;
        }

        public async Task<TeacherPerformanceReportIndexViewModel>
            GetReportAsync(
                TeacherPerformanceReportFilterViewModel filter)
        {
            NormalizeFilter(filter);

            /*
             * Lấy cấu hình Teacher Performance đang active.
             */
            var activePerformanceConfiguration =
                await _context.TeacherPerformanceConfigurations
                    .AsNoTracking()
                    .Where(configuration =>
                        configuration.IsActive)
                    .OrderByDescending(configuration =>
                        configuration.UpdatedAt)
                    .Select(configuration => new
                    {
                        configuration.FeedbackWeight,
                        configuration.CompletionWeight,
                        configuration.StudentPerformanceWeight,
                        configuration.MinimumEvaluationCount
                    })
                    .FirstOrDefaultAsync();

            bool hasActivePerformanceConfiguration =
                activePerformanceConfiguration != null;

            decimal feedbackWeight =
                activePerformanceConfiguration
                    ?.FeedbackWeight
                ?? 0.60000m;

            decimal completionWeight =
                activePerformanceConfiguration
                    ?.CompletionWeight
                ?? 0.20000m;

            decimal studentPerformanceWeight =
                activePerformanceConfiguration
                    ?.StudentPerformanceWeight
                ?? 0.20000m;

            int minimumEvaluationCount =
                activePerformanceConfiguration
                    ?.MinimumEvaluationCount
                ?? 5;

            /*
             * Lấy cấu hình Student Ranking đang active.
             */
            var activeRankingConfiguration =
                await _context.RankingConfigurations
                    .AsNoTracking()
                    .Where(configuration =>
                        configuration.IsActive)
                    .OrderByDescending(configuration =>
                        configuration.UpdatedAt)
                    .Select(configuration => new
                    {
                        configuration.AssignmentWeight,
                        configuration.QuizWeight,
                        configuration.LessonProgressWeight
                    })
                    .FirstOrDefaultAsync();

            bool hasActiveRankingConfiguration =
                activeRankingConfiguration != null;

            decimal assignmentWeight =
                activeRankingConfiguration
                    ?.AssignmentWeight
                ?? 0.50000m;

            decimal quizWeight =
                activeRankingConfiguration
                    ?.QuizWeight
                ?? 0.30000m;

            decimal lessonProgressWeight =
                activeRankingConfiguration
                    ?.LessonProgressWeight
                ?? 0.20000m;

            /*
             * Lấy danh sách tài khoản có role TEACHER.
             */
            var teacherQuery =
                from user in _context.Users.AsNoTracking()

                join account in _context.Accounts.AsNoTracking()
                    on user.UserId equals account.UserId

                join role in _context.Roles.AsNoTracking()
                    on account.RoleId equals role.RoleId

                where role.RoleName == RoleNames.Teacher

                select new
                {
                    TeacherId = user.UserId,
                    user.FullName,
                    user.Email,
                    account.Username,
                    user.AvatarUrl,
                    UserStatus = user.Status,
                    AccountIsActive = account.IsActive
                };

            if (!string.IsNullOrWhiteSpace(
                filter.SearchText))
            {
                string keyword =
                    filter.SearchText.Trim();

                teacherQuery =
                    teacherQuery.Where(teacher =>
                        teacher.FullName.Contains(keyword)
                        || teacher.Email.Contains(keyword)
                        || teacher.Username.Contains(keyword));
            }

            var teachers =
                await teacherQuery
                    .OrderBy(teacher =>
                        teacher.FullName)
                    .ToListAsync();

            List<long> teacherIds =
                teachers
                    .Select(teacher =>
                        teacher.TeacherId)
                    .ToList();

            if (teacherIds.Count == 0)
            {
                return CreateEmptyViewModel(
                    filter,
                    hasActivePerformanceConfiguration,
                    hasActiveRankingConfiguration,
                    feedbackWeight,
                    completionWeight,
                    studentPerformanceWeight,
                    assignmentWeight,
                    quizWeight,
                    lessonProgressWeight,
                    minimumEvaluationCount);
            }

            /*
             * Lấy toàn bộ Classroom thuộc các Teacher.
             */
            List<ClassroomMetricRow> classrooms =
                await _context.Classrooms
                    .AsNoTracking()
                    .Where(classroom =>
                        teacherIds.Contains(
                            classroom.TeacherId))
                    .Select(classroom =>
                        new ClassroomMetricRow
                        {
                            ClassroomId =
                                classroom.ClassroomId,

                            TeacherId =
                                classroom.TeacherId,

                            CourseId =
                                classroom.CourseId,

                            Status =
                                classroom.Status
                        })
                    .ToListAsync();

            /*
             * Tính Feedback Score.
             */
            var feedbackRows =
                await (
                    from evaluation in
                        _context.TeacherEvaluations
                            .AsNoTracking()

                    join classroom in
                        _context.Classrooms.AsNoTracking()
                        on evaluation.ClassroomId
                        equals classroom.ClassroomId

                    where teacherIds.Contains(
                        classroom.TeacherId)

                    group evaluation
                        by classroom.TeacherId
                        into evaluationGroup

                    select new
                    {
                        TeacherId =
                            evaluationGroup.Key,

                        EvaluationCount =
                            evaluationGroup.Count(),

                        AverageRating =
                            evaluationGroup.Average(
                                evaluation =>
                                    (decimal)
                                    evaluation.OverallRating)
                    }
                ).ToListAsync();

            Dictionary<long, int>
                evaluationCountByTeacher =
                    feedbackRows.ToDictionary(
                        row => row.TeacherId,
                        row => row.EvaluationCount);

            Dictionary<long, decimal>
                feedbackScoreByTeacher =
                    feedbackRows.ToDictionary(
                        row => row.TeacherId,
                        row => ClampPercentage(
                            row.AverageRating
                            / 5m
                            * 100m));

            /*
             * Tính các thành phần Student Performance.
             */
            Dictionary<long, decimal>
                assignmentScoreByTeacher =
                    await GetAssignmentScoresAsync(
                        teacherIds);

            Dictionary<long, decimal>
                quizScoreByTeacher =
                    await GetQuizScoresAsync(
                        teacherIds);

            Dictionary<long, decimal>
                lessonProgressScoreByTeacher =
                    await GetLessonProgressScoresAsync(
                        classrooms);

            List<TeacherPerformanceReportItemViewModel>
                reportItems = new();

            foreach (var teacher in teachers)
            {
                List<ClassroomMetricRow>
                    teacherClassrooms =
                        classrooms
                            .Where(classroom =>
                                classroom.TeacherId
                                == teacher.TeacherId)
                            .ToList();

                int totalClassroomCount =
                    teacherClassrooms.Count;

                /*
                 * DRAFT và CANCELLED không được tính vào
                 * Completion Score.
                 */
                int eligibleClassroomCount =
                    teacherClassrooms.Count(classroom =>
                        classroom.Status == "OPEN"
                        || classroom.Status == "IN_PROGRESS"
                        || classroom.Status == "COMPLETED");

                int completedClassroomCount =
                    teacherClassrooms.Count(classroom =>
                        classroom.Status == "COMPLETED");

                decimal completionScore =
                    eligibleClassroomCount == 0
                        ? 0m
                        : ClampPercentage(
                            completedClassroomCount
                            * 100m
                            / eligibleClassroomCount);

                int evaluationCount =
                    evaluationCountByTeacher
                        .GetValueOrDefault(
                            teacher.TeacherId,
                            0);

                decimal feedbackScore =
                    feedbackScoreByTeacher
                        .GetValueOrDefault(
                            teacher.TeacherId,
                            0m);

                decimal assignmentScore =
                    assignmentScoreByTeacher
                        .GetValueOrDefault(
                            teacher.TeacherId,
                            0m);

                decimal quizScore =
                    quizScoreByTeacher
                        .GetValueOrDefault(
                            teacher.TeacherId,
                            0m);

                decimal lessonProgressScore =
                    lessonProgressScoreByTeacher
                        .GetValueOrDefault(
                            teacher.TeacherId,
                            0m);

                decimal studentPerformanceScore =
                    ClampPercentage(
                        assignmentScore
                            * assignmentWeight
                        + quizScore
                            * quizWeight
                        + lessonProgressScore
                            * lessonProgressWeight);

                decimal overallScore =
                    ClampPercentage(
                        feedbackScore
                            * feedbackWeight
                        + completionScore
                            * completionWeight
                        + studentPerformanceScore
                            * studentPerformanceWeight);

                reportItems.Add(
                    new TeacherPerformanceReportItemViewModel
                    {
                        TeacherId =
                            teacher.TeacherId,

                        FullName =
                            teacher.FullName,

                        Email =
                            teacher.Email,

                        Username =
                            teacher.Username,

                        AvatarUrl =
                            teacher.AvatarUrl,

                        UserStatus =
                            teacher.UserStatus,

                        IsAccountActive =
                            teacher.AccountIsActive,

                        TotalClassroomCount =
                            totalClassroomCount,

                        EligibleClassroomCount =
                            eligibleClassroomCount,

                        CompletedClassroomCount =
                            completedClassroomCount,

                        EvaluationCount =
                            evaluationCount,

                        MinimumEvaluationCount =
                            minimumEvaluationCount,

                        FeedbackScore =
                            RoundScore(
                                feedbackScore),

                        CompletionScore =
                            RoundScore(
                                completionScore),

                        AssignmentScore =
                            RoundScore(
                                assignmentScore),

                        QuizScore =
                            RoundScore(
                                quizScore),

                        LessonProgressScore =
                            RoundScore(
                                lessonProgressScore),

                        StudentPerformanceScore =
                            RoundScore(
                                studentPerformanceScore),

                        OverallScore =
                            RoundScore(
                                overallScore)
                    });
            }

            int totalTeacherCount =
                reportItems.Count;

            int sufficientDataCount =
                reportItems.Count(teacher =>
                    teacher.HasSufficientEvaluationData);

            int insufficientDataCount =
                totalTeacherCount
                - sufficientDataCount;

            List<TeacherPerformanceReportItemViewModel>
                sufficientTeachers =
                    reportItems
                        .Where(teacher =>
                            teacher.HasSufficientEvaluationData)
                        .ToList();

            decimal averageOverallScore =
                sufficientTeachers.Count == 0
                    ? 0m
                    : RoundScore(
                        sufficientTeachers.Average(
                            teacher =>
                                teacher.OverallScore));

            TeacherPerformanceReportItemViewModel?
                topTeacher =
                    sufficientTeachers
                        .OrderByDescending(teacher =>
                            teacher.OverallScore)
                        .ThenBy(teacher =>
                            teacher.FullName)
                        .FirstOrDefault();

            IEnumerable<TeacherPerformanceReportItemViewModel>
                filteredItems = reportItems;

            if (filter.DataStatus ==
                TeacherPerformanceDataStatuses.Sufficient)
            {
                filteredItems =
                    filteredItems.Where(teacher =>
                        teacher.HasSufficientEvaluationData);
            }
            else if (filter.DataStatus ==
                TeacherPerformanceDataStatuses.Insufficient)
            {
                filteredItems =
                    filteredItems.Where(teacher =>
                        !teacher.HasSufficientEvaluationData);
            }

            filteredItems =
                filteredItems
                    .OrderByDescending(teacher =>
                        teacher.HasSufficientEvaluationData)
                    .ThenByDescending(teacher =>
                        teacher.OverallScore)
                    .ThenBy(teacher =>
                        teacher.FullName);

            int totalItems =
                filteredItems.Count();

            int totalPages =
                totalItems == 0
                    ? 1
                    : (int)Math.Ceiling(
                        totalItems
                        / (double)filter.PageSize);

            if (filter.Page > totalPages)
            {
                filter.Page = totalPages;
            }

            List<TeacherPerformanceReportItemViewModel>
                pagedItems =
                    filteredItems
                        .Skip(
                            (filter.Page - 1)
                            * filter.PageSize)
                        .Take(filter.PageSize)
                        .ToList();

            return new TeacherPerformanceReportIndexViewModel
            {
                Filter =
                    filter,

                Teachers =
                    pagedItems,

                TotalItems =
                    totalItems,

                TotalPages =
                    totalPages,

                TotalTeacherCount =
                    totalTeacherCount,

                SufficientDataCount =
                    sufficientDataCount,

                InsufficientDataCount =
                    insufficientDataCount,

                AverageOverallScore =
                    averageOverallScore,

                TopTeacherName =
                    topTeacher?.FullName,

                TopTeacherScore =
                    topTeacher?.OverallScore,

                HasActivePerformanceConfiguration =
                    hasActivePerformanceConfiguration,

                HasActiveRankingConfiguration =
                    hasActiveRankingConfiguration,

                FeedbackWeightPercentage =
                    feedbackWeight * 100m,

                CompletionWeightPercentage =
                    completionWeight * 100m,

                StudentPerformanceWeightPercentage =
                    studentPerformanceWeight * 100m,

                AssignmentWeightPercentage =
                    assignmentWeight * 100m,

                QuizWeightPercentage =
                    quizWeight * 100m,

                LessonProgressWeightPercentage =
                    lessonProgressWeight * 100m,

                MinimumEvaluationCount =
                    minimumEvaluationCount
            };
        }

        public async Task<TeacherPerformanceDetailsViewModel?>
            GetDetailsAsync(long teacherId)
        {
            if (teacherId <= 0)
            {
                return null;
            }

            var teacher =
                await (
                    from user in
                        _context.Users.AsNoTracking()

                    join account in
                        _context.Accounts.AsNoTracking()
                        on user.UserId
                        equals account.UserId

                    join role in
                        _context.Roles.AsNoTracking()
                        on account.RoleId
                        equals role.RoleId

                    where user.UserId == teacherId
                          && role.RoleName
                          == RoleNames.Teacher

                    select new
                    {
                        TeacherId =
                            user.UserId,

                        user.FullName,

                        user.Email,

                        account.Username,

                        user.AvatarUrl,

                        UserStatus =
                            user.Status,

                        AccountIsActive =
                            account.IsActive
                    }
                ).FirstOrDefaultAsync();

            if (teacher == null)
            {
                return null;
            }

            /*
             * Tái sử dụng GetReportAsync để số liệu tổng
             * trên Index và Details luôn giống nhau.
             */
            TeacherPerformanceReportIndexViewModel report =
                await GetReportAsync(
                    new TeacherPerformanceReportFilterViewModel
                    {
                        SearchText =
                            teacher.Email,

                        DataStatus =
                            TeacherPerformanceDataStatuses.All,

                        Page = 1,

                        PageSize = 100
                    });

            TeacherPerformanceReportItemViewModel? summary =
                report.Teachers.FirstOrDefault(item =>
                    item.TeacherId == teacherId);

            summary ??=
                new TeacherPerformanceReportItemViewModel
                {
                    TeacherId =
                        teacher.TeacherId,

                    FullName =
                        teacher.FullName,

                    Email =
                        teacher.Email,

                    Username =
                        teacher.Username,

                    AvatarUrl =
                        teacher.AvatarUrl,

                    UserStatus =
                        teacher.UserStatus,

                    IsAccountActive =
                        teacher.AccountIsActive,

                    MinimumEvaluationCount =
                        report.MinimumEvaluationCount
                };

            List<TeacherPerformanceClassroomViewModel>
                classrooms =
                    await (
                        from classroom in
                            _context.Classrooms
                                .AsNoTracking()

                        join course in
                            _context.Courses
                                .AsNoTracking()
                            on classroom.CourseId
                            equals course.CourseId

                        where classroom.TeacherId
                              == teacherId

                        orderby classroom.StartDate
                            descending

                        select new
                            TeacherPerformanceClassroomViewModel
                        {
                            ClassroomId =
                                    classroom.ClassroomId,

                            CourseId =
                                    classroom.CourseId,

                            ClassName =
                                    classroom.ClassName,

                            CourseCode =
                                    course.CourseCode,

                            CourseTitle =
                                    course.Title,

                            Status =
                                    classroom.Status,

                            StartDate =
                                    classroom.StartDate,

                            EndDate =
                                    classroom.EndDate
                        }
                    ).ToListAsync();

            List<long> classroomIds =
                classrooms
                    .Select(classroom =>
                        classroom.ClassroomId)
                    .ToList();

            List<TeacherEvaluationItemViewModel>
                evaluations = new();

            if (classroomIds.Count > 0)
            {
                evaluations =
                    await (
                        from evaluation in
                            _context.TeacherEvaluations
                                .AsNoTracking()

                        join classroom in
                            _context.Classrooms
                                .AsNoTracking()
                            on evaluation.ClassroomId
                            equals classroom.ClassroomId

                        join student in
                            _context.Users
                                .AsNoTracking()
                            on evaluation.StudentId
                            equals student.UserId

                        where classroomIds.Contains(
                            evaluation.ClassroomId)

                        orderby evaluation.CreatedAt
                            descending

                        select new
                            TeacherEvaluationItemViewModel
                        {
                            TeacherEvaluationId =
                                    evaluation
                                        .TeacherEvaluationId,

                            ClassroomId =
                                    evaluation.ClassroomId,

                            ClassroomName =
                                    classroom.ClassName,

                            StudentId =
                                    evaluation.StudentId,

                            StudentName =
                                    student.FullName,

                            StudentEmail =
                                    student.Email,

                            ContentClarityRating =
                                    evaluation
                                        .ContentClarityRating,

                            SupportRating =
                                    evaluation.SupportRating,

                            FeedbackQualityRating =
                                    evaluation
                                        .FeedbackQualityRating,

                            ClassOrganizationRating =
                                    evaluation
                                        .ClassOrganizationRating,

                            OverallRating =
                                    evaluation.OverallRating,

                            Comment =
                                    evaluation.Comment,

                            IsCommentHidden =
                                    evaluation.IsCommentHidden,

                            CreatedAt =
                                    evaluation.CreatedAt
                        }
                    ).ToListAsync();
            }

            Dictionary<long,
                List<TeacherEvaluationItemViewModel>>
                evaluationsByClassroom =
                    evaluations
                        .GroupBy(evaluation =>
                            evaluation.ClassroomId)
                        .ToDictionary(
                            group => group.Key,
                            group => group.ToList());

            List<EnrollmentMetricRow> enrollments =
                classroomIds.Count == 0
                    ? new List<EnrollmentMetricRow>()
                    : await _context.Enrollments
                        .AsNoTracking()
                        .Where(enrollment =>
                            classroomIds.Contains(
                                enrollment.ClassroomId)
                            && enrollment.Status
                                != "DROPPED")
                        .Select(enrollment =>
                            new EnrollmentMetricRow
                            {
                                EnrollmentId =
                                    enrollment.EnrollmentId,

                                ClassroomId =
                                    enrollment.ClassroomId,

                                Status =
                                    enrollment.Status
                            })
                        .ToListAsync();

            Dictionary<long, decimal>
                assignmentScoresByClassroom =
                    await GetAssignmentScoresByClassroomAsync(
                        classroomIds);

            Dictionary<long, decimal>
                quizScoresByClassroom =
                    await GetQuizScoresByClassroomAsync(
                        classroomIds);

            Dictionary<long, decimal>
                lessonScoresByClassroom =
                    await GetLessonScoresByClassroomAsync(
                        classrooms,
                        enrollments);

            foreach (
                TeacherPerformanceClassroomViewModel classroom
                in classrooms)
            {
                List<EnrollmentMetricRow>
                    classroomEnrollments =
                        enrollments
                            .Where(enrollment =>
                                enrollment.ClassroomId
                                == classroom.ClassroomId)
                            .ToList();

                classroom.EnrollmentCount =
                    classroomEnrollments.Count;

                classroom.CompletedEnrollmentCount =
                    classroomEnrollments.Count(enrollment =>
                        enrollment.Status
                        == "COMPLETED");

                List<TeacherEvaluationItemViewModel>
                    classroomEvaluations =
                        evaluationsByClassroom
                            .GetValueOrDefault(
                                classroom.ClassroomId,
                                new List<
                                    TeacherEvaluationItemViewModel>());

                classroom.EvaluationCount =
                    classroomEvaluations.Count;

                decimal averageRating =
                    classroomEvaluations.Count == 0
                        ? 0m
                        : classroomEvaluations.Average(
                            evaluation =>
                                (decimal)
                                evaluation.OverallRating);

                classroom.FeedbackScore =
                    RoundScore(
                        ClampPercentage(
                            averageRating
                            / 5m
                            * 100m));

                classroom.CompletionScore =
                    classroom.Status == "COMPLETED"
                        ? 100m
                        : 0m;

                classroom.AssignmentScore =
                    assignmentScoresByClassroom
                        .GetValueOrDefault(
                            classroom.ClassroomId,
                            0m);

                classroom.QuizScore =
                    quizScoresByClassroom
                        .GetValueOrDefault(
                            classroom.ClassroomId,
                            0m);

                classroom.LessonProgressScore =
                    lessonScoresByClassroom
                        .GetValueOrDefault(
                            classroom.ClassroomId,
                            0m);

                classroom.StudentPerformanceScore =
                    RoundScore(
                        ClampPercentage(
                            classroom.AssignmentScore
                                * report
                                    .AssignmentWeightPercentage
                                / 100m
                            + classroom.QuizScore
                                * report
                                    .QuizWeightPercentage
                                / 100m
                            + classroom.LessonProgressScore
                                * report
                                    .LessonProgressWeightPercentage
                                / 100m));

                classroom.OverallScore =
                    RoundScore(
                        ClampPercentage(
                            classroom.FeedbackScore
                                * report
                                    .FeedbackWeightPercentage
                                / 100m
                            + classroom.CompletionScore
                                * report
                                    .CompletionWeightPercentage
                                / 100m
                            + classroom
                                .StudentPerformanceScore
                                * report
                                    .StudentPerformanceWeightPercentage
                                / 100m));
            }

            return new TeacherPerformanceDetailsViewModel
            {
                Summary =
                    summary,

                Classrooms =
                    classrooms,

                Evaluations =
                    evaluations,

                FeedbackWeightPercentage =
                    report.FeedbackWeightPercentage,

                CompletionWeightPercentage =
                    report.CompletionWeightPercentage,

                StudentPerformanceWeightPercentage =
                    report.StudentPerformanceWeightPercentage,

                AssignmentWeightPercentage =
                    report.AssignmentWeightPercentage,

                QuizWeightPercentage =
                    report.QuizWeightPercentage,

                LessonProgressWeightPercentage =
                    report.LessonProgressWeightPercentage,

                AverageContentClarityRating =
                    GetAverageRating(
                        evaluations,
                        evaluation =>
                            evaluation.ContentClarityRating),

                AverageSupportRating =
                    GetAverageRating(
                        evaluations,
                        evaluation =>
                            evaluation.SupportRating),

                AverageFeedbackQualityRating =
                    GetAverageRating(
                        evaluations,
                        evaluation =>
                            evaluation.FeedbackQualityRating),

                AverageClassOrganizationRating =
                    GetAverageRating(
                        evaluations,
                        evaluation =>
                            evaluation.ClassOrganizationRating),

                AverageOverallRating =
                    GetAverageRating(
                        evaluations,
                        evaluation =>
                            evaluation.OverallRating)
            };
        }

        /*
         * Assignment Score theo Teacher.
         */
        private async Task<Dictionary<long, decimal>>
            GetAssignmentScoresAsync(
                List<long> teacherIds)
        {
            if (teacherIds.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            var scoreRows =
                await (
                    from grade in
                        _context.SubmissionGrades
                            .AsNoTracking()

                    join submission in
                        _context.AssignmentSubmissions
                            .AsNoTracking()
                        on grade.AssignmentSubmissionId
                        equals submission
                            .AssignmentSubmissionId

                    join classroomAssignment in
                        _context.ClassroomAssignments
                            .AsNoTracking()
                        on submission.ClassroomAssignmentId
                        equals classroomAssignment
                            .ClassroomAssignmentId

                    join classroom in
                        _context.Classrooms.AsNoTracking()
                        on classroomAssignment.ClassroomId
                        equals classroom.ClassroomId

                    join template in
                        _context.AssignmentTemplates
                            .AsNoTracking()
                        on classroomAssignment
                            .AssignmentTemplateId
                        equals template
                            .AssignmentTemplateId

                    where teacherIds.Contains(
                              classroom.TeacherId)
                          && template.TotalScore > 0m

                    select new
                    {
                        classroom.TeacherId,

                        Percentage =
                            grade.Score
                            * 100m
                            / template.TotalScore
                    }
                ).ToListAsync();

            return scoreRows
                .GroupBy(row =>
                    row.TeacherId)
                .ToDictionary(
                    group => group.Key,
                    group => RoundScore(
                        group.Average(row =>
                            ClampPercentage(
                                row.Percentage))));
        }

        /*
         * Quiz Score theo Teacher.
         * Lấy attempt cao nhất của từng Student
         * trong từng Classroom Quiz.
         */
        private async Task<Dictionary<long, decimal>>
            GetQuizScoresAsync(
                List<long> teacherIds)
        {
            if (teacherIds.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            var bestAttemptRows =
                await (
                    from attempt in
                        _context.QuizAttempts
                            .AsNoTracking()

                    join classroomQuiz in
                        _context.ClassroomQuizzes
                            .AsNoTracking()
                        on attempt.ClassroomQuizId
                        equals classroomQuiz
                            .ClassroomQuizId

                    join classroom in
                        _context.Classrooms
                            .AsNoTracking()
                        on classroomQuiz.ClassroomId
                        equals classroom.ClassroomId

                    where teacherIds.Contains(
                              classroom.TeacherId)
                          && attempt.TotalScore != null

                    group attempt by new
                    {
                        classroom.TeacherId,
                        attempt.StudentId,
                        attempt.ClassroomQuizId
                    }
                    into attemptGroup

                    select new
                    {
                        attemptGroup.Key.TeacherId,

                        BestScore =
                            attemptGroup.Max(attempt =>
                                attempt.TotalScore)
                    }
                ).ToListAsync();

            return bestAttemptRows
                .Where(row =>
                    row.BestScore.HasValue)
                .GroupBy(row =>
                    row.TeacherId)
                .ToDictionary(
                    group => group.Key,
                    group => RoundScore(
                        group.Average(row =>
                            ClampPercentage(
                                row.BestScore!.Value
                                * 10m))));
        }

        /*
         * Lesson Progress Score theo Teacher.
         */
        private async Task<Dictionary<long, decimal>>
            GetLessonProgressScoresAsync(
                List<ClassroomMetricRow> classrooms)
        {
            if (classrooms.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            List<long> classroomIds =
                classrooms
                    .Select(classroom =>
                        classroom.ClassroomId)
                    .ToList();

            List<long> courseIds =
                classrooms
                    .Select(classroom =>
                        classroom.CourseId)
                    .Distinct()
                    .ToList();

            Dictionary<long, int>
                publishedLessonCountByCourse =
                    await (
                        from module in
                            _context.Modules.AsNoTracking()

                        join lesson in
                            _context.Lessons.AsNoTracking()
                            on module.ModuleId
                            equals lesson.ModuleId

                        where courseIds.Contains(
                                  module.CourseId)
                              && lesson.Status
                                  == "PUBLISHED"

                        group lesson
                            by module.CourseId
                            into lessonGroup

                        select new
                        {
                            CourseId =
                                lessonGroup.Key,

                            LessonCount =
                                lessonGroup.Count()
                        }
                    ).ToDictionaryAsync(
                        row => row.CourseId,
                        row => row.LessonCount);

            List<EnrollmentMetricRow> enrollments =
                await _context.Enrollments
                    .AsNoTracking()
                    .Where(enrollment =>
                        classroomIds.Contains(
                            enrollment.ClassroomId)
                        && enrollment.Status
                            != "DROPPED")
                    .Select(enrollment =>
                        new EnrollmentMetricRow
                        {
                            EnrollmentId =
                                enrollment.EnrollmentId,

                            ClassroomId =
                                enrollment.ClassroomId,

                            Status =
                                enrollment.Status
                        })
                    .ToListAsync();

            if (enrollments.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            List<long> enrollmentIds =
                enrollments
                    .Select(enrollment =>
                        enrollment.EnrollmentId)
                    .ToList();

            Dictionary<long, int>
                completedLessonCountByEnrollment =
                    await (
                        from progress in
                            _context.LessonProgresses
                                .AsNoTracking()

                        join enrollment in
                            _context.Enrollments
                                .AsNoTracking()
                            on progress.EnrollmentId
                            equals enrollment.EnrollmentId

                        join classroom in
                            _context.Classrooms
                                .AsNoTracking()
                            on enrollment.ClassroomId
                            equals classroom.ClassroomId

                        join lesson in
                            _context.Lessons
                                .AsNoTracking()
                            on progress.LessonId
                            equals lesson.LessonId

                        join module in
                            _context.Modules
                                .AsNoTracking()
                            on lesson.ModuleId
                            equals module.ModuleId

                        where enrollmentIds.Contains(
                                  progress.EnrollmentId)
                              && progress.IsCompleted
                              && lesson.Status
                                  == "PUBLISHED"
                              && module.CourseId
                                  == classroom.CourseId

                        group progress
                            by progress.EnrollmentId
                            into progressGroup

                        select new
                        {
                            EnrollmentId =
                                progressGroup.Key,

                            CompletedCount =
                                progressGroup.Count()
                        }
                    ).ToDictionaryAsync(
                        row => row.EnrollmentId,
                        row => row.CompletedCount);

            Dictionary<long, ClassroomMetricRow>
                classroomById =
                    classrooms.ToDictionary(
                        classroom =>
                            classroom.ClassroomId);

            List<(long TeacherId, decimal Score)>
                enrollmentScores = new();

            foreach (
                EnrollmentMetricRow enrollment
                in enrollments)
            {
                if (!classroomById.TryGetValue(
                    enrollment.ClassroomId,
                    out ClassroomMetricRow? classroom))
                {
                    continue;
                }

                int totalLessons =
                    publishedLessonCountByCourse
                        .GetValueOrDefault(
                            classroom.CourseId,
                            0);

                int completedLessons =
                    completedLessonCountByEnrollment
                        .GetValueOrDefault(
                            enrollment.EnrollmentId,
                            0);

                decimal score =
                    totalLessons == 0
                        ? 0m
                        : ClampPercentage(
                            completedLessons
                            * 100m
                            / totalLessons);

                enrollmentScores.Add(
                    (
                        classroom.TeacherId,
                        score
                    ));
            }

            return enrollmentScores
                .GroupBy(row =>
                    row.TeacherId)
                .ToDictionary(
                    group => group.Key,
                    group => RoundScore(
                        group.Average(row =>
                            row.Score)));
        }

        /*
         * Assignment Score theo Classroom.
         */
        private async Task<Dictionary<long, decimal>>
            GetAssignmentScoresByClassroomAsync(
                List<long> classroomIds)
        {
            if (classroomIds.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            var rows =
                await (
                    from grade in
                        _context.SubmissionGrades
                            .AsNoTracking()

                    join submission in
                        _context.AssignmentSubmissions
                            .AsNoTracking()
                        on grade.AssignmentSubmissionId
                        equals submission
                            .AssignmentSubmissionId

                    join classroomAssignment in
                        _context.ClassroomAssignments
                            .AsNoTracking()
                        on submission.ClassroomAssignmentId
                        equals classroomAssignment
                            .ClassroomAssignmentId

                    join template in
                        _context.AssignmentTemplates
                            .AsNoTracking()
                        on classroomAssignment
                            .AssignmentTemplateId
                        equals template
                            .AssignmentTemplateId

                    where classroomIds.Contains(
                              classroomAssignment
                                  .ClassroomId)
                          && template.TotalScore > 0m

                    select new
                    {
                        classroomAssignment.ClassroomId,

                        ScorePercentage =
                            grade.Score
                            * 100m
                            / template.TotalScore
                    }
                ).ToListAsync();

            return rows
                .GroupBy(row =>
                    row.ClassroomId)
                .ToDictionary(
                    group => group.Key,
                    group => RoundScore(
                        group.Average(row =>
                            ClampPercentage(
                                row.ScorePercentage))));
        }

        /*
         * Quiz Score theo Classroom.
         */
        private async Task<Dictionary<long, decimal>>
            GetQuizScoresByClassroomAsync(
                List<long> classroomIds)
        {
            if (classroomIds.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            var bestAttemptRows =
                await (
                    from attempt in
                        _context.QuizAttempts
                            .AsNoTracking()

                    join classroomQuiz in
                        _context.ClassroomQuizzes
                            .AsNoTracking()
                        on attempt.ClassroomQuizId
                        equals classroomQuiz
                            .ClassroomQuizId

                    where classroomIds.Contains(
                              classroomQuiz.ClassroomId)
                          && attempt.TotalScore != null

                    group attempt by new
                    {
                        classroomQuiz.ClassroomId,
                        attempt.StudentId,
                        attempt.ClassroomQuizId
                    }
                    into attemptGroup

                    select new
                    {
                        attemptGroup.Key.ClassroomId,

                        BestScore =
                            attemptGroup.Max(attempt =>
                                attempt.TotalScore)
                    }
                ).ToListAsync();

            return bestAttemptRows
                .Where(row =>
                    row.BestScore.HasValue)
                .GroupBy(row =>
                    row.ClassroomId)
                .ToDictionary(
                    group => group.Key,
                    group => RoundScore(
                        group.Average(row =>
                            ClampPercentage(
                                row.BestScore!.Value
                                * 10m))));
        }

        /*
         * Lesson Progress Score theo Classroom.
         */
        private async Task<Dictionary<long, decimal>>
            GetLessonScoresByClassroomAsync(
                List<TeacherPerformanceClassroomViewModel>
                    classrooms,
                List<EnrollmentMetricRow> enrollments)
        {
            if (classrooms.Count == 0
                || enrollments.Count == 0)
            {
                return new Dictionary<long, decimal>();
            }

            List<long> courseIds =
                classrooms
                    .Select(classroom =>
                        classroom.CourseId)
                    .Distinct()
                    .ToList();

            List<long> enrollmentIds =
                enrollments
                    .Select(enrollment =>
                        enrollment.EnrollmentId)
                    .ToList();

            Dictionary<long, int>
                publishedLessonCountByCourse =
                    await (
                        from module in
                            _context.Modules.AsNoTracking()

                        join lesson in
                            _context.Lessons.AsNoTracking()
                            on module.ModuleId
                            equals lesson.ModuleId

                        where courseIds.Contains(
                                  module.CourseId)
                              && lesson.Status
                                  == "PUBLISHED"

                        group lesson
                            by module.CourseId
                            into lessonGroup

                        select new
                        {
                            CourseId =
                                lessonGroup.Key,

                            LessonCount =
                                lessonGroup.Count()
                        }
                    ).ToDictionaryAsync(
                        row => row.CourseId,
                        row => row.LessonCount);

            Dictionary<long, int>
                completedLessonsByEnrollment =
                    await (
                        from progress in
                            _context.LessonProgresses
                                .AsNoTracking()

                        join enrollment in
                            _context.Enrollments
                                .AsNoTracking()
                            on progress.EnrollmentId
                            equals enrollment.EnrollmentId

                        join classroom in
                            _context.Classrooms
                                .AsNoTracking()
                            on enrollment.ClassroomId
                            equals classroom.ClassroomId

                        join lesson in
                            _context.Lessons
                                .AsNoTracking()
                            on progress.LessonId
                            equals lesson.LessonId

                        join module in
                            _context.Modules
                                .AsNoTracking()
                            on lesson.ModuleId
                            equals module.ModuleId

                        where enrollmentIds.Contains(
                                  progress.EnrollmentId)
                              && progress.IsCompleted
                              && lesson.Status
                                  == "PUBLISHED"
                              && module.CourseId
                                  == classroom.CourseId

                        group progress
                            by progress.EnrollmentId
                            into progressGroup

                        select new
                        {
                            EnrollmentId =
                                progressGroup.Key,

                            CompletedCount =
                                progressGroup.Count()
                        }
                    ).ToDictionaryAsync(
                        row => row.EnrollmentId,
                        row => row.CompletedCount);

            Dictionary<long,
                TeacherPerformanceClassroomViewModel>
                classroomById =
                    classrooms.ToDictionary(
                        classroom =>
                            classroom.ClassroomId);

            List<(long ClassroomId, decimal Score)>
                scores = new();

            foreach (
                EnrollmentMetricRow enrollment
                in enrollments)
            {
                if (!classroomById.TryGetValue(
                    enrollment.ClassroomId,
                    out TeacherPerformanceClassroomViewModel?
                        classroom))
                {
                    continue;
                }

                int publishedLessonCount =
                    publishedLessonCountByCourse
                        .GetValueOrDefault(
                            classroom.CourseId,
                            0);

                int completedLessonCount =
                    completedLessonsByEnrollment
                        .GetValueOrDefault(
                            enrollment.EnrollmentId,
                            0);

                decimal score =
                    publishedLessonCount == 0
                        ? 0m
                        : ClampPercentage(
                            completedLessonCount
                            * 100m
                            / publishedLessonCount);

                scores.Add(
                    (
                        classroom.ClassroomId,
                        score
                    ));
            }

            return scores
                .GroupBy(row =>
                    row.ClassroomId)
                .ToDictionary(
                    group => group.Key,
                    group => RoundScore(
                        group.Average(row =>
                            row.Score)));
        }

        private static decimal GetAverageRating(
            List<TeacherEvaluationItemViewModel>
                evaluations,
            Func<TeacherEvaluationItemViewModel, byte>
                selector)
        {
            if (evaluations.Count == 0)
            {
                return 0m;
            }

            return RoundScore(
                evaluations.Average(evaluation =>
                    (decimal)
                    selector(evaluation)));
        }

        private static decimal ClampPercentage(
            decimal value)
        {
            if (value < 0m)
            {
                return 0m;
            }

            if (value > 100m)
            {
                return 100m;
            }

            return value;
        }

        private static decimal RoundScore(
            decimal value)
        {
            return decimal.Round(
                value,
                2,
                MidpointRounding.AwayFromZero);
        }

        private static void NormalizeFilter(
            TeacherPerformanceReportFilterViewModel
                filter)
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
                TeacherPerformanceDataStatuses.All,
                TeacherPerformanceDataStatuses.Sufficient,
                TeacherPerformanceDataStatuses.Insufficient
            };

            if (!validStatuses.Contains(
                filter.DataStatus))
            {
                filter.DataStatus =
                    TeacherPerformanceDataStatuses.All;
            }
        }

        private static
            TeacherPerformanceReportIndexViewModel
            CreateEmptyViewModel(
                TeacherPerformanceReportFilterViewModel
                    filter,
                bool hasPerformanceConfiguration,
                bool hasRankingConfiguration,
                decimal feedbackWeight,
                decimal completionWeight,
                decimal studentPerformanceWeight,
                decimal assignmentWeight,
                decimal quizWeight,
                decimal lessonProgressWeight,
                int minimumEvaluationCount)
        {
            return new TeacherPerformanceReportIndexViewModel
            {
                Filter =
                    filter,

                TotalPages = 1,

                HasActivePerformanceConfiguration =
                    hasPerformanceConfiguration,

                HasActiveRankingConfiguration =
                    hasRankingConfiguration,

                FeedbackWeightPercentage =
                    feedbackWeight * 100m,

                CompletionWeightPercentage =
                    completionWeight * 100m,

                StudentPerformanceWeightPercentage =
                    studentPerformanceWeight * 100m,

                AssignmentWeightPercentage =
                    assignmentWeight * 100m,

                QuizWeightPercentage =
                    quizWeight * 100m,

                LessonProgressWeightPercentage =
                    lessonProgressWeight * 100m,

                MinimumEvaluationCount =
                    minimumEvaluationCount
            };
        }
    }
}