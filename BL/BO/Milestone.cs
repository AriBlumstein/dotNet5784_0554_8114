

namespace BO;

public class Milestone
{
    int ID { get; init; }

    String Name { get; init; }

    String Description { get; init; }
    DateTime Created { get; init; }
    Status Status { get; init; }
    DateTime ActualStart { get; init; }
    DateTime ActualEnd { get; init; }
    Double Progress { get; init; }
    String? Notes { get; init; }

    TaskList Dependencies{ get; init; } 



}
