



namespace Dal;
using DalApi;

sealed internal class DalList : IDal
{
    private static readonly Lazy<DalList> lazyInstance = new Lazy<DalList>(() => new DalList(), LazyThreadSafetyMode.ExecutionAndPublication); //for lazy initialization and thread safety, part of system

    public static DalList Instance {get {return lazyInstance.Value;}}

    private DalList() { }
    static DalList() { }
   
    


    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IConfig Config => new ConfigImplementation();

    public void Reset()
    {
        Task.Reset();
        Engineer.Reset();
        Dependency.Reset();
        Config.Reset();
    }
}
