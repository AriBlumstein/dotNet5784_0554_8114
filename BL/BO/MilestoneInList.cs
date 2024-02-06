

namespace BO;

public class MilestoneInList
{
    public int ID { get; init; }
    public String Description { get; init; }
    public String Name { get; init; }

    public DateTime Created { get; init; }

    public Status? Status { get; init; }

    public Double? Progress { get; init; }

    public override string ToString() => this.ToStringProperty();

}
