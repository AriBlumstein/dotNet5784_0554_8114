

namespace BO;

public class MilestoneInList
{
    int ID { get; init; }
    String Description { get; set; }
    String Name { get; set; }

    DateTime Created { get; set; }

    Status Status { get; set; }

    Double? Progress { get; set; }

}
