
namespace DO;


/// <summary>
/// data object to represent a task
/// </summary>

public record Task
{

    /// <summary>
    /// parameter constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="nickName"></param>
    /// <param name="description"></param>
    /// <param name="milestone"></param>
    /// <param name="created"></param>
    /// <param name="projectedStart"></param>
    /// <param name="actualStart"></param>
    /// <param name="deadline"></param>
    /// <param name="duration"></param>
    /// <param name="actualEnd"></param>
    /// <param name="deliverable"></param>
    /// <param name="notes"></param>
    /// <param name="assignedEngineer"></param>
    /// <param name="difficulty"></param>
    public Task(int id, string nickName, string description, bool milestone, DateTime created, DateTime? projectedStart, DateTime? actualStart, DateTime? deadline, int? duration, DateTime? actualEnd, string? deliverable, string? notes, int? assignedEngineer, Experience? difficulty)
    {
        ID = id;
        NickName = nickName;
        Description = description;
        Milestone = milestone;
        Created = created;
        ProjectedStart = projectedStart;
        ActualStart = actualStart;
        Deadline = deadline;
        Duration = duration;
        ActualEnd = actualEnd;
        Deliverable = deliverable;
        Notes = notes;
        AssignedEngineer = assignedEngineer;
        Difficulty = difficulty;
    }

    public Task() { ID = ++_cur; }

    public int ID { get; init; }
    public String NickName { get; init; }
    public String Description {get; init;}
    public Boolean Milestone {get; init;}
    public DateTime Created {get; init;}
    public DateTime? ProjectedStart { get; init; } = null;
    public DateTime? ActualStart { get; init; } = null;
    public DateTime? Deadline { get; init; } = null;
    public int? Duration { get; init; } = null;
    public DateTime? ActualEnd { get; init; } = null;
    public String? Deliverable { get; init; } = null;
    public String? Notes { get; init; } = null;
    public int? AssignedEngineer { get; init; } = null;
    public Experience? Difficulty { get; init; } = null;


   

}
