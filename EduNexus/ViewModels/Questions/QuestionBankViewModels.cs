using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduNexus.ViewModels.Questions
{
    public class QuestionBankListViewModel
    {
        public long BankId { get; set; }
        public string BankName { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsPublic { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int QuestionCount { get; set; }
    }

    public class QuestionBankFormViewModel
    {
        public long BankId { get; set; }
        
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public long CourseId { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        [StringLength(200)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; } = null!;

        public string? Description { get; set; }

        [Display(Name = "Public")]
        public bool IsPublic { get; set; }
    }
}
