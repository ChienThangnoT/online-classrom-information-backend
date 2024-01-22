using System;
using System.Collections.Generic;

namespace LMSystem.Repository.Models;

public partial class StepCompleted
{
    public string CompletedStepId { get; set; }

    public string RegistrationId { get; set; }

    public DateTime? DateCompleted { get; set; }

    public virtual RegistrationCourse Registration { get; set; }
}
