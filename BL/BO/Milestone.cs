

namespace BO;

public class Milestone
{
    public int ID { get; init; }
    public String Name { get; set; }
    public String Description { get; set; }
    public DateTime Created { get; set; }
    public Status? Status { get; set; }
    public DateTime? ActualStart { get; set; }
    public DateTime? ActualEnd { get; set; }
    public Double? Progress { get; set; }
    public String? Notes { get; set; }

    public List<TaskInList> Dependencies{ get; set; } 



}
