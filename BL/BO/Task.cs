

using System.ComponentModel.DataAnnotations;


namespace BO;
using System;


internal class Task
{
    int ID { get; }
    String Descripiton { get; set; }
    DateTime Created { get; }
    Status Status { get; set; }

    //TaskList Dependencies {get;}= new TaskList()

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
