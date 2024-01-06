using Dal;
using DalApi;
namespace DalTest;

internal class Program
{

    private static ITask? s_dalTask = new TaskImplementation();
    private static IEngineer? s_dalEngineer = new EngineerImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();
    static void Main(string[] args)
    {
        Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
        Console.WriteLine("Hello, World!");
    }
}