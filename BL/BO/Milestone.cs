

namespace BO;

public class Milestone
{
    int ID { get; }

    String Name { get; set; }

    String Description { get; set; }
    DateTime Created { get; }
    Status Status { get; set; }
    DateTime ActualStart { get; set; }
    DateTime ActualEnd { get; set; }
    Double Progress { get; set; }
    String? Notes { get; set; }

    TaskList Dependencies{ get; } = new TaskList();



}
