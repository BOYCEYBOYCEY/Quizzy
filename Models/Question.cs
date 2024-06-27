using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Quizzy_Main.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    [Required(ErrorMessage = "Question is Required")]
    public string? Question1 { get; set; }
    [Required(ErrorMessage = "Please enter Option1")]
    public string? Option1 { get; set; }
    [Required(ErrorMessage = "Please enter Option2")]
    public string? Option2 { get; set; }
   /* [Required(ErrorMessage = "Please enter Option3")]*/
    public string? Option3 { get; set; }
   /* [Required(ErrorMessage = "Please enter Option4")]*/
    public string? Option4 { get; set; }


    [Required(ErrorMessage = "Enter the correct option")]
    
    public string CorrectOption { get; set; }

    public int? TopicId { get; set; } 

    public string? DifficultyLevel { get; set; } 

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();  

    public virtual Topic? Topic { get; set; } 
}
