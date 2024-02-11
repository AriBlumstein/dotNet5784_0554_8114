


namespace Dal;

using DalApi;


sealed internal class DalXml : IDal
{
   private static readonly Lazy<DalXml> _lazyInstance = new Lazy<DalXml>(() => new DalXml()); //for lazy initialization and thread safety, part of system

    public static DalXml Instance {get {return _lazyInstance.Value;}}


    private DalXml() { }
    static DalXml() { }
    public ITask Task => new Dal.TaskImplementation();

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
