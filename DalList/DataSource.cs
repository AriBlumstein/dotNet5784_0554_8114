﻿


namespace Dal;

using DO;

/// <summary>
/// database for the ifferent entities
/// </summary>

internal static class DataSource
{

    internal static readonly Random random = new Random(); 

    /// <summary>
    /// databases for the different attributes
    /// </summary>
    internal static List<Dependency> Dependencies { get; } = new();
    internal static List<Engineer> Engineers { get; } = new();

    internal static List<Task> Tasks { get; } = new();


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

        internal static DateTime? projectStart=null;

        internal static DateTime? projectEnd = null;

    }




}
