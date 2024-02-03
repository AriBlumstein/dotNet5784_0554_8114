

namespace BlApi;

public interface IMilestone
{
    /// <summary>
    /// create the schedule
    /// </summary>
    void CreateProjectSchedule();

    /// <summary>
    /// get the milestones info
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    BO.Milestone Read(int id);


    /// <summary>
    /// update a given milestone
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    BO.Milestone Update(int id);
}
