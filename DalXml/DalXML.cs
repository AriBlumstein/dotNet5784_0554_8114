

using DalApi;

namespace DalXml;

using DalApi;

sealed public class DalXML : IDal
{
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
