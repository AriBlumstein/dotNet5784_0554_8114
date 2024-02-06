

namespace BO;



public class Engineer
{
    public int ID { get; init; }
    public String Name { get; set; }
    public String Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double Cost { get; set; }
    public TaskInEngineer? Task { get; set; }

    public override string ToString() => this.ToStringProperty();
   
}
