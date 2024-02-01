

using System.ComponentModel.DataAnnotations;


namespace BO;
using System;


internal class Task
{
    int ID { get; }
    String Descripiton { get; init; }
    DateTime Created { get; init; }
    Status Status { get; init }

    TaskList Dependencies { get; init; }

    //MilestoneInClass Milestone {get;}=new MilestoneInClass()


    DateTime? ProjectedStart { get; set; }

    DateTime? ActualStart { get; set; }

    private DateTime? _projetcedEnd=null;
    DateTime? ProjectedEnd { get; }

    DateTime? ActualEnd { get; set; }
    DateTime? Deadline { get; set; }
    String? Deliverable { get; set; }
    String? Notes { get; set; }

    //EngineerInTask Engineer 

    EngineerExperience Complexity { get; set; }




}









    



}
