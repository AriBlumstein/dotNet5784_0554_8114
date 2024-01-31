



namespace Dal;
using DalApi;
using System.ComponentModel;

sealed internal class DalList : IDal
{
    static DalList Instance { get => LazyInit.Instance;  }

    class LazyInit
    {
        internal static readonly DalList Instance = new DalList();
        static LazyInit() {}
    }
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
