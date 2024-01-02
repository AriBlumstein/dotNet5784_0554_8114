
namespace DO;

public record Task
{

    private static int _cur=0; //used for assigning id

    public Task(DateTime created, DateTime? projectedStart, DateTime actualStart, DateTime deadline, int duration, DateTime actualEnd, string? nickName=null, string description="", bool milestone=false, string deliverable="", string? notes=null, int assignedEngineer=-1, Experience difficulty=Experience.Beginner)
    {
        ID= ++_cur;
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

    public Task() { };

    public int ID { get; set; }
    public String? NickName { set; get; }
    public String Description { set; get; }
    public Boolean Milestone { set; get; }
    public DateTime Created { set; get; }
    public DateTime? ProjectedStart { set; get; }
    public DateTime ActualStart { set; get; }
    public DateTime Deadline { set; get; }
    public int Duration { set; get; }
    public DateTime ActualEnd { set; get; }
    public String Deliverable { set; get; }
    public String? Notes { set; get; }
    public int AssignedEngineer { set; get; }
    public Experience Difficulty { set; get; }


   

}
