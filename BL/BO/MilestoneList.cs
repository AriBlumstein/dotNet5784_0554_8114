

namespace BO;

public class MilestoneList
{
    String Description { get; init; }
    String Name { get; init; }

    DateTime Created { get; init; }

    Status? Status { get; init; }

    Double? Progress { get; init; }

}
