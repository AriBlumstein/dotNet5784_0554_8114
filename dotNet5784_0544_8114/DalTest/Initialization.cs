
namespace DalTest;
using DO;
using DalApi;

/// <summary>
/// class to initialize the databases for the purpose of the test
/// </summary>
public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;

    private static readonly Random s_rand= new ();

    private static void createTasks() { }

    /// <summary>
    /// initalize engineers
    /// </summary>
    private static void createEngineers() {

        const int MIN_ID = 200000000, MAX_ID = 400000000;

        String[] names = new string[] { "Ariel", "Eliyahu", "Benji", "Binyamin", "David" };
        foreach (var _name in names)
        {
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer!.Read(_id) != null);
            
            Experience _e = (Experience)s_rand.Next(0,Enum.GetValues(typeof(Experience)).Length);

            double rate=(double)s_rand.Next(200,10000);


            s_dalEngineer!.Create(new Engineer(_id, _name, rate, _name + "@company.com", _e));
        }
    }
}

    private static void createDependencies() { }

}
