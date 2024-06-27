using System;
using System.Collections.Generic;

namespace Quizzy_Main.Models;

public partial class Topic
{
    public Topic()
    {
        TopicId = 0;
        TopicName = "default";

    }
    public int TopicId { get; set; }

    public string? TopicName { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
