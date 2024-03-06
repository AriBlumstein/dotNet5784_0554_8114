

namespace BlImplementation;
using BlApi;
using DalApi;

internal class Bl : IBl
{

    private static Lazy<Bl> _lazyInstance=new Lazy<Bl>(()=>new Bl());

    private Bl() 
    {
        //thread safe clock update
        var timer = new System.Timers.Timer();
        timer.Interval = 1000; // 1 second
        timer.Elapsed += (sender, e) =>
        {
            using (var mutex = new Mutex(true, "ClockMutex")) //making a mutex and I get it first upon initialization
            {
                Clock = Clock.AddSeconds(1);
            }
        };
        timer.Start();


    }
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

  

    private  static DateTime s_Clock = DateTime.Now;

    public DateTime Clock
    {
        get { return s_Clock; }
        private set { s_Clock = value; }
    }



    public void MoveForwardHour()
    {
        using (var mutex = new Mutex(false, "ClockMutex")) //making a mutex and I get it first upon initialization
        {
            Clock = Clock.AddHours(1);
        }
        
    }

    public void MoveForwardDay()
    {
        using (var mutex = new Mutex(false, "ClockMutex")) //making a mutex and I get it first upon initialization
        {
            Clock = Clock.AddDays(1);
        }
     
    }

    public void TimeReset()
    {
        using (var mutex = new Mutex(false, "ClockMutex")) //making a mutex and I get it first upon initialization
        {
            Clock = DateTime.Now;
        }
        
    }



}
