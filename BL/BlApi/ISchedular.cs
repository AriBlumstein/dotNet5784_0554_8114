

namespace BlApi;

/// <summary>
/// a scheduling mechanism that will automatically create a schedule for our task
/// </summary>
public interface ISchedular
{
    /// <summary>
    /// create a schedule for the project at hand
    /// </summary>
    /// <param name="projectedStart"></param>
    void createSchedule(DateTime projectedStart);
}
