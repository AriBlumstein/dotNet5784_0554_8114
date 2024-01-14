
namespace Dal;
using DalApi;

internal class ConfigImplementation: IConfig
{
    public void setProjectStart(DateTime start)
    {
        DataSource.Config.projectStart = start;
    }
    public void setProjectEnd(DateTime end)
    {
        DataSource.Config.projectEnd = end;
    }


    public DateTime getProjectStart()
    { return DataSource.Config.projectStart ?? throw new Exception("no start time"); }
    public DateTime getProjectEnd()
    { return DataSource.Config.projectEnd ?? throw new Exception("no end time"); }

    //reset the project
    public void Reset()
    {
        DataSource.Config.projectStart = null;
        DataSource.Config.projectEnd = null;
    }

}
