﻿

namespace BlApi;

/// <summary>
/// a scheduling mechanism that will automatically create a schedule for our task
/// </summary>
public interface ISchedular
{
    /// <summary>
    /// create a schedule for the project at hand
    /// </summary>
    /// <param name="projectedStart"></param>
    void createSchedule(DateTime projectedStart);


    /// <summary>
    /// method that checks if we are already in production
    /// </summary>
    /// <returns>if there is a project start date</returns>
    bool InProduction();


    /// <summary>
    /// method that returns the project startdate
    /// </summary>
    /// <returns></returns>

    DateTime GetProjectStartDate();

    
}
