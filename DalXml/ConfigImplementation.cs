

using Dal;
using DalApi;
using DO;

namespace DalXml;

/// <summary>
/// access to the internal Config class, specifically for the datetime attributes to give access to the outside
/// </summary>
internal class ConfigImplementation : IConfig
{
    private readonly string configName = "data-config"; //the xml file name

    private readonly DateTime baseReset = new DateTime(1998, 12, 31); //our base reset
   
    
    /// <summary>
    /// get the ProjectEnd
    /// </summary>
    /// <returns>DateTime</returns>
    /// <exception cref="DALConfigDateNotSet"></exception>
    public DateTime getProjectEnd()
    {
        if (Config.ProjectEnd == baseReset||Config.ProjectStart==null)//for first time or subsequent times after it was reset
            throw new DALConfigDateNotSet("No date set for the project end date yet");

        return Config.ProjectEnd!.Value;
    }


    /// <summary>
    /// get the projectStart
    /// </summary>
    /// <returns>DateTime</returns>
    /// <exception cref="DALConfigDateNotSet"></exception>
    public DateTime getProjectStart()
    {

        if (Config.ProjectStart == baseReset||Config.ProjectStart==null) //after first time or
            throw new DALConfigDateNotSet("No date set for the project start date yet");

        return Config.ProjectStart!.Value;
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
    public void setProjectEnd(DateTime end)
    {
        try
        {
            getProjectStart();
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
    public void setProjectStart(DateTime start)
    {
        try
        {
            getProjectEnd();
            throw new IllegalConfigAccessException("Cannot set end date after it was already set");
        }
        catch(Exception) 
        {
            Config.ProjectStart = start;
        }
    }
}
