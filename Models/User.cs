using System;
using System.Collections.Generic;

namespace Quizzy_Main.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? UserRole { get; set; }

    public string? token { get; set; }

    public bool? newsletter { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

}
