
namespace DO;


/// <summary>
/// task to be done
/// </summary>
/// <param name="ID"></param>
/// <param name="Nickname"></param>
/// <param name="Description"></param>
/// <param name="Milestone"></param>
/// <param name="Created"></param>
/// <param name="ProjectedStart"></param>
/// <param name="ActualStart"></param>
/// <param name="Deadline"></param>
/// <param name="Duration"></param>
/// <param name="ActualEnd"></param>
/// <param name="Deliverable"></param>
/// <param name="Notes"></param>
/// <param name="AssignedEngineer"></param>
/// <param name="Difficulty"></param>

public record Task
(
     int ID,
     String Nickname, 
     String Description,
     Boolean Milestone, 
     DateTime Created,
     DateTime? ProjectedStart  = null,
     DateTime? ActualStart  = null,
     DateTime? Deadline  = null,
     int? Duration  = null,
     DateTime? ActualEnd  = null,
     String? Deliverable  = null,
     String? Notes  = null,
     int? AssignedEngineer  = null,
     Experience? Difficulty  = null,
     bool Active=true

)
{
    public Task() : this(0, "", "", false, DateTime.Now) { }

    public override string ToString()
    {
        return $"""
            ID: {ID}
            Nickname: {Nickname ?? "null"}
            Description: {Description ?? "null"}
            Milestone: {Milestone}
            Date Created: {Created}
            Projected Start: {(ProjectedStart.HasValue ? ProjectedStart : "null")}
            Actual Start: {(ActualStart.HasValue ? ActualStart : "null")}
            Deadline: {(Deadline.HasValue ? Deadline : "null")}
            Duration: {(Duration.HasValue ? Duration : "null")}
            Actual End: {ActualEnd.ToString() ?? "null"}
            Deliverable: {Deliverable ?? "null"}
            Notes: {Notes ?? "null"}
            Assigned Engineer: {AssignedEngineer.ToString() ?? "null"}
            Difficulty: {Difficulty.ToString() ?? "null"}
            """;
    }
}
