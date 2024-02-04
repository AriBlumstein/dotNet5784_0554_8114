

namespace BO;
using System;


public class Task
{
    public int ID { get; init; }
    public String Descripiton { get; set; }
    public DateTime Created { get; init; }
    public Status? Status { get; set; }
    public List<TaskInList> Dependencies { get; set; }
    public MilestoneInTask? Milestone {get; set;}
    public DateTime? ProjectedStart { get; set; }

    public DateTime? ActualStart { get; set; }
    public DateTime? ProjectedEnd { get; set; }
    public DateTime? ActualEnd { get; set; }
    public DateTime? Deadline { get; set; }
    public String? Deliverable { get; set; }
    public String? Notes { get; set; }
    public EngineerInTask? Engineer {  get; set; }
    public EngineerExperience? Complexity { get; set; }

}



