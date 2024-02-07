
namespace Dal;
using DalApi;
using DO;

internal class ConfigImplementation: IConfig
{
    public void SetProjectStart(DateTime start)
    {
        if(DataSource.Config.projectStart!=null)// we dont't want to reset the date unless it was not set
        {
            throw new IllegalConfigAccessException("Can't set new date");
        }
        DataSource.Config.projectStart = start;
    }
    public void SetProjectEnd(DateTime end)
    {
        if (DataSource.Config.projectEnd!=null)
        {
            throw new IllegalConfigAccessException("Can't set new date");
        }
        DataSource.Config.projectEnd = end;
    }


    public DateTime GetProjectStart()
    { return DataSource.Config.projectStart ?? throw new DALConfigDateNotSet("no start time"); }
    public DateTime GetProjectEnd()
    { return DataSource.Config.projectEnd ?? throw new DALConfigDateNotSet("no end time"); }

    //reset the project
    public void Reset()
    {
        DataSource.Config.projectStart = null;
        DataSource.Config.projectEnd = null;

        //we can now set new date
    }

}
