

namespace BO;

public class Milestone
{
    int ID { get; init; }
    String Name { get; set; }
    String Description { get; set; }
    DateTime Created { get; set; }
    Status? Status { get; set; }
    DateTime? ActualStart { get; set; }
    DateTime? ActualEnd { get; set; }
    Double? Progress { get; set; }
    String? Notes { get; set; }

    List<TaskinList> Dependencies{ get; init; } 



}
