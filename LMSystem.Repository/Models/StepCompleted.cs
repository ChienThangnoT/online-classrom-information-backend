using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class StepCompleted
{
    public int CompletedStepId { get; set; }

    public int RegistrationId { get; set; }
    public int StepId { get; set; } // Newly added field

    public DateTime? DateCompleted { get; set; }

    public virtual RegistrationCourse? Registration { get; set; }
    public virtual ICollection<AnswerHistory> AnswerHistories { get; set; } = new List<AnswerHistory>();

}
