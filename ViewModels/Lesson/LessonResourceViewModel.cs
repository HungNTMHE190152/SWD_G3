using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace EduNexus.ViewModels.Lesson;

public class LessonResourceViewModel
{
    public long LessonResourceId { get; set; }

    public long LessonId { get; set; }

    [Required]
    public string ResourceName { get; set; } = "";

    [Required]
    public string ResourceType { get; set; } = "PDF";

    
    public string ResourceUrl { get; set; } = "";


    public int DisplayOrder { get; set; }

    public IFormFile? UploadFile { get; set; }
}