

namespace BlImplementation;
using BlApi;
using DalApi;

internal class Bl : IBl
{
    private IDal _dal = DalApi.Factory.Get;
    
    public BlApi.IEngineer Engineer => new EngineerImplementation();

    public BlApi.IMilestone Milestone => new MilestoneImplementation();

    public BlApi.ITask Task => new TaskImplementation();

    public BlApi.ISchedular Schedular => new SimpleSchedularImplementation();

    public void Reset()
    {
        _dal.Reset();
    }

    
}
