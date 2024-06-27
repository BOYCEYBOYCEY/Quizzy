using System;
using System.Collections.Generic;

namespace Quizzy_Main.Models;

public partial class Result
{
    public int ResultId { get; set; }

    public int? ExamId { get; set; }

    public int? QuestionId { get; set; }

    public string? SelectedOption { get; set; }

    public string? CorrectOption { get; set; }

    public int? Result1 { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Question? Question { get; set; }
}
