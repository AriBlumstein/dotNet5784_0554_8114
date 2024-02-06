
using System.Reflection;

namespace BO;

public class MilestoneInTask
{
    public int ID { get; init; }
    public String Alias { get; init; }

    public override string ToString() => this.ToStringProperty();

}
