
using DO;

namespace Dal;

/// <summary>
/// database for the ifferent entities
/// </summary>

internal static class DataSource
{

    internal static readonly Random random = new Random(); 

    /// <summary>
    /// databases for the different attributes
    /// </summary>
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();

    internal static List<DO.Task> Tasks { get; } = new();


    internal class Config
    {   

        /// <summary>
        /// automatic ids for task and dependency as well as an arbitrary start and and date
        /// </summary>
        
        internal const int startdependencyID = 1;

        private static int nextDependencyID = startdependencyID;
        internal static int NextDependencyID { get => nextDependencyID++; }

        internal const int startTaskID = 1;

        internal static int nextTaskID = startTaskID;
        internal static int NextTaskID { get => nextTaskID++; }

        internal static DateTime? projectStart=new DateTime(2007, 1, 1);

        internal static DateTime? projectEnd = new DateTime(2018, 1, 1);

    }


}
