

namespace BO;
using System;


public class Task
{
    int ID { get; init; }
    String Descripiton { get; set; }
    DateTime Created { get; init; }
    Status? Status { get; set; }
    List<TaskinList> Dependencies { get; init; }
    MilestoneInTask? Milestone {get; set;}
    DateTime? ProjectedStart { get; set; }

    DateTime? ActualStart { get; set; }
    DateTime? ProjectedEnd { get; set; }
    DateTime? ActualEnd { get; set; }
    DateTime? Deadline { get; set; }
    String? Deliverable { get; set; }
    String? Notes { get; set; }
    EngineerInTask? Engineer {  get; set; }
    EngineerExperience? Complexity { get; set; }

}









    



}
