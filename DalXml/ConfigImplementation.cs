



namespace Dal;

using DalApi;
using DO;

/// <summary>
/// access to the internal Config class, specifically for the datetime attributes to give access to the outside
/// </summary>
internal class ConfigImplementation : IConfig
{
    private readonly string configName = "data-config"; //the xml file name

    
    /// <summary>
    /// get the ProjectEnd
    /// </summary>
    /// <returns>DateTime</returns>
    /// <exception cref="DALConfigDateNotSet"></exception>
    public DateTime GetProjectEnd()
    {
        return Config.ProjectEnd ?? throw new DALConfigDateNotSet("project end was not set yet");
    }


    /// <summary>
    /// get the projectStart
    /// </summary>
    /// <returns>DateTime</returns>
    /// <exception cref="DALConfigDateNotSet"></exception>
    public DateTime GetProjectStart()
    {

        return Config.ProjectStart ?? throw new DALConfigDateNotSet("Project start date not yet set");
    }

    /// <summary>
    /// reset the dates to "null"
    /// </summary>
    public void Reset()
    {
        XMLTools.ClearDates(configName);

    }

    /// <summary>
    /// set the project End
    /// </summary>
    /// <param name="end"></param>
    /// <exception cref="IllegalConfigAccessException"></exception>
    public void SetProjectEnd(DateTime end)
    {
        try
        {
            GetProjectEnd();
            throw new IllegalConfigAccessException("Cannot set start date after it was already set");
        }
        catch (Exception)
        {
            Config.ProjectEnd = end;
        }
    }


    /// <summary>
    /// set the project start
    /// </summary>
    /// <param name="start"></param>
    /// <exception cref="IllegalConfigAccessException"></exception>
    public void SetProjectStart(DateTime start)
    {
        try
        {
            GetProjectStart();
            throw new IllegalConfigAccessException("Cannot set end date after it was already set");
        }
        catch(Exception) 
        {
            Config.ProjectStart = start;
        }
    }
}
