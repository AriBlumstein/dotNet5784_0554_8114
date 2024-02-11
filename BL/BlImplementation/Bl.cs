

namespace BlImplementation;
using BlApi;
using DalApi;

internal class Bl : IBl
{

    private static Lazy<Bl> _lazyInstance=new Lazy<Bl>(()=>new Bl());

    private Bl() { }
    static Bl() { }
    public static Bl Instance { get { return _lazyInstance.Value; } }

   
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
