

namespace BlApi;

/// <summary>
/// master interface for the api interfaces
/// </summary>
public interface IBl
{


    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public ITask Task { get; }

}
