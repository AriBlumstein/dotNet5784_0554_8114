

using System.Runtime.CompilerServices;

namespace BlApi;

/// <summary>
/// master interface for the api interfaces
/// </summary>
public interface IBl
{
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public ITask Task { get; }

    public ISchedular Schedular { get; }


    /// <summary>
    /// start a new project
    /// </summary>
    public void Reset();


    public DateTime Clock { get; }

    public void MoveForwardHour();

    public void MoveForwardDay();

    public void TimeReset();

}
