

namespace BlApi;

/// <summary>
/// a scheduling mechanism that will automatically create a schedule for our task
/// </summary>
public interface ISchedular
{
    void createSchecule(DateTime projectedStart);
}
