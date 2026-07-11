using EduNexus.Data;
using EduNexus.Models;
using EduNexus.Services.Interfaces;
using EduNexus.ViewModels.Student;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using System.Text.Json;

namespace EduNexus.Services.Implementations
{
    public class AIGradingService : IAIGradingService
    {
        private readonly EduNexusContext _context;
        private readonly IConfiguration _configuration;

        public AIGradingService(EduNexusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AISubmissionResultViewModel> GradeSubmissionAsync(long submissionId)
        {
            var submission = await _context.EssaySubmissions
                .Include(s => s.Assignment)
                .Include(s => s.Assignment.Rubrics)
                    .ThenInclude(r => r.RubricCriteria)
                .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

            if (submission == null)
                throw new InvalidOperationException("Không tìm thấy bài nộp.");

            var rubric = submission.Assignment.Rubrics.FirstOrDefault();
            if (rubric == null)
                throw new InvalidOperationException("Assignment chưa có Rubric.");

            var prompt = BuildPrompt(submission, rubric);

            var apiKey = _configuration["OpenAI:ApiKey"]
                ?? throw new InvalidOperationException("Chưa cấu hình OpenAI API Key.");

            var chatClient = new ChatClient("gpt-4o-mini", apiKey);

            var result = await chatClient.CompleteChatAsync(new List<ChatMessage>
            {
                new SystemChatMessage("Bạn là giảng viên đại học nghiêm túc, khách quan và công bằng."),
                new UserChatMessage(prompt)
            });

            var completion = result.Value;


            var responseText = completion.Content[0].Text;
            var aiResult = JsonSerializer.Deserialize<AIGradeResponse>(responseText);

            // Lưu kết quả AI
            //submission.AIScore = aiResult?.OverallScore ?? 0;
            //submission.AIFeedback = aiResult?.OverallFeedback;
            //submission.AIModel = "gpt-4o-mini";
            //submission.AIGradedAt = DateTime.UtcNow;
            submission.GradingStatus = "AIGRADED";

            // === PHẦN CUỐI CỦA METHOD GradeSubmissionAsync ===
            await _context.SaveChangesAsync();

            // Trả về ViewModel
            return new AISubmissionResultViewModel
            {
                SubmissionId = submission.SubmissionId,
                AssignmentId = submission.AssignmentId,
                AssignmentTitle = submission.Assignment.Title,
                //AIScore = (decimal)submission.AIScore,
                MaxScore = submission.Assignment.TotalScore,
                //AIFeedback = submission.AIFeedback ?? "Không có feedback.",
                //AIGradedAt = submission.AIGradedAt,
                Status = submission.GradingStatus ?? "AIGRADED",

                CriterionResults = aiResult?.CriterionResults?.Select(c => new AICriterionResultViewModel
                {
                    CriterionName = c.CriterionName,
                    Score = c.Score,
                    MaxScore = c.MaxScore,
                    Comment = c.Comment
                }).ToList() ?? new()
            };
        }

        private string BuildPrompt(EssaySubmission submission, Rubric rubric)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Assignment: {submission.Assignment.Title}");
            sb.AppendLine($"Rubric: {rubric.RubricName}");
            sb.AppendLine("Criteria:");

            foreach (var c in rubric.RubricCriteria)
            {
                sb.AppendLine($"- {c.CriterionName} (Weight: {c.Weight}%, Max: {c.MaxScore}) - {c.Description}");
            }

            sb.AppendLine("\nStudent Answer:");
            sb.AppendLine(submission.SubmissionText ?? "No content provided.");

            sb.AppendLine("\nReturn JSON only:");
            sb.AppendLine("{\"overallScore\":8.5,\"overallFeedback\":\"...\",\"criterionResults\":[{\"criterionName\":\"...\",\"score\":4.0,\"maxScore\":5.0,\"comment\":\"...\"}]}");

            return sb.ToString();
        }
    }

    public class AIGradeResponse
    {
        public decimal OverallScore { get; set; }
        public string OverallFeedback { get; set; } = string.Empty;
        public List<AICriterionResponse> CriterionResults { get; set; } = new();
    }

    public class AICriterionResponse
    {
        public string CriterionName { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}