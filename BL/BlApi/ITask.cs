﻿

namespace BlApi;

public interface ITask
{
    /// <summary>
    /// return a list of Tasks based on the filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns><IEnumerable<BO.Task></BO.Task></returns>
    IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter=null);

    /// <summary>
    /// Returns a task based on the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    BO.Task Read(int id);

    /// <summary>
    /// create a task
    /// </summary>
    /// <param name="task"></param>
    /// <returns>the id of the created task</returns>
    BO.Task Create(BO.Task task);

    /// <summary>
    /// update a task 
    /// </summary>
    /// <param name="task"></param>
    /// <returns>the task that was updated</returns>
    BO.Task Update(BO.Task task);


    /// <summary>
    /// delete a task, only in production
    /// </summary>
    /// <param name="id"></param>
    void Delete(int id);


    /// <summary>
    /// specifically to update the projected start date in planning
    /// </summary>
    /// <param name="id"></param>
    /// <param name="startDate"></param>
    BO.Task UpdateProjectedStartDate(int id, DateTime startDate);


    /// <summary>
    /// gets the list of dependencies as tasks in list that were not yet completed
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    IEnumerable<BO.TaskInList> ReadUncompletedDependencies(BO.Task task);
}
