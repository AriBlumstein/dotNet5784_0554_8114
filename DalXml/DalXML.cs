

using DalApi;

namespace DalXml;

using DalApi;
using System.Diagnostics;

sealed internal class DalXML : IDal
{
    static DalXML Instance { get => LazyInit.Instance; }

    class LazyInit
    {
        internal static readonly DalXML Instance = new DalXML();
        static LazyInit() { }
    }
    private DalXML() { }
    static DalXML() { }
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
