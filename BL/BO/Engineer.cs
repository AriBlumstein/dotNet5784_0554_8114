

namespace BO;



public class Engineer
{
    int ID { get; init; }
    String Name { get; set; }
    EngineerExperience Level { get; set; }
    double Cost { get; set; }
    TaskInEngineer? Task { get; set; }

}
