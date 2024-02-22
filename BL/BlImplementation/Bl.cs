

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
    
    public BlApi.IEngineer Engineer => new EngineerImplementation(this);

    public BlApi.IMilestone Milestone => new MilestoneImplementation();

    public BlApi.ITask Task => new TaskImplementation(this);

    public BlApi.ISchedular Schedular => new SimpleSchedularImplementation(this);

    public void Reset()
    {
        _dal.Reset();
    }

  

    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock
    {
        get { return s_Clock; }
        private set { s_Clock = value; }
    }



    public void MoveForwardHour()
    {
        Clock.AddHours(1);
    }

    public void MoveForwardDay()
    {
        Clock.AddDays(1);
    }

    public void TimeReset()
    {
        Clock= DateTime.Now;
    }



}
