
namespace Dal;

/// <summary>
/// database for the ifferent entities
/// </summary>

internal static class DataSource
{

    internal static Random random = new Random();
    internal static List<DO.Dependency> Dependencies{ get; } = new ();
    internal static List<DO.Engineer> Engineers { get; } = new ();

    internal static List<DO.Task> Tasks { get; } = new ();


    internal class Config
    {
        internal static int dependencyID = 1;
        internal static int NextdepencyID { get => dependencyID++; }

        internal static int taskID = 1;
        internal static int TaskID { get => dependencyID++; }
    }

}
