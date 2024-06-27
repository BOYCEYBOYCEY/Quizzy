using System;
using System.Collections.Generic;

namespace Quizzy_Main.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public DateTime? ExamStartDateTime { get; set; }

    public DateTime? ExamEndDateTime { get; set; }

    public int? ExamDuration { get; set; }

    public int? NoOfAttempts { get; set; }

    public int? UserId { get; set; }

    public int? TopicId { get; set; }

    public string? DifficultyLevel { get; set; }

    public double? Finalscore { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual Topic? Topic { get; set; }

    public virtual User? User { get; set; }

    
}
