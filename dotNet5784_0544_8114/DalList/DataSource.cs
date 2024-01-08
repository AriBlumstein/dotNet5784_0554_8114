
using DO;

namespace Dal;

/// <summary>
/// database for the ifferent entities
/// </summary>

internal static class DataSource
{

    internal static Random random = new Random();
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();

    internal static List<DO.Task> Tasks { get; } = new();


    internal class Config
    {
        internal static int dependencyID = 1;
        internal static int NextDependencyID { get => dependencyID++; }

        internal static int taskID = 1;
        internal static int NextTaskID { get => dependencyID++; }

        internal static DateTime? projectStart=new DateTime(2007, 1, 1);

        internal static DateTime? projectEnd = new DateTime(2018, 1, 1);

    }

    internal static bool itemExists(int id, Type type)
    {
       
        bool exists = false;
        if (type == typeof(DO.Dependency))
        {
            exists= Dependencies.Exists(d=> d.ID == id&&d.Active);
        }
        else if (type == typeof(DO.Engineer)) {
            exists= Engineers.Exists(d => d.ID == id&&d.Active);
        }
        
        else if (type == typeof(DO.Task))
        {
            exists= Tasks.Exists(d => d.ID == id && d.Active);
        }
        return exists;
     
    }

}
