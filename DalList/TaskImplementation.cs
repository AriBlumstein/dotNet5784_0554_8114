


namespace Dal;

using DalApi;
using DO;
using System.Linq;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// Implementation of create function to add a task to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new task</returns>
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskID;
        
        Task localItem = item with { ID = id };

        DataSource.Tasks.Add(localItem);
        return id;
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id"></param>

    public void Delete(int id)
    {
        Task cur = Read(id);

        int index = DataSource.Tasks.IndexOf(cur);

        DataSource.Tasks[index] = cur with { Active = false };
    }



    /// <summary>
    /// Returns a reference to a Task object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the task if it exists, null otherwise</returns>
    ///  /// <exception cref="DalDoesNotExistException"></exception>
    public Task Read(int id)
    {
        Task? cur = (DataSource.Tasks.FirstOrDefault(i => i.ID == id && i.Active));

        return cur ?? throw new DalDoesNotExistException($"Task with ID={id} does not exist");
    }

    /// <summary>
    /// Return a copy of all the Tasks
    /// </summary>
    /// <returns>An IEnumerable of tasks</returns>
 
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   where IsActive(item) //make sure to only return active items
                   select item;
        }
        return from item in DataSource.Tasks
               where IsActive(item)  //make sure to only return active items
               select item;

    }



    /// <summary>
    /// Update a task 
    /// </summary>
    /// <param name="item"></param>
    public void Update(Task item)
    {
        Task cur = Read(item.ID); //find the original item

        int index = DataSource.Tasks.IndexOf(cur); //find its index

        DataSource.Tasks[index] = item; //eliminate original, add new one
    }


    /// <summary>
    /// reset the tasks
    /// </summary>
    public void Reset()
    {
        DataSource.Tasks.Clear();
    }


    /// <summary>
    /// returns if the entity in question is active
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool IsActive(Task t)
    {
        return t.Active;
    }


    /// <summary>
    /// read based on a filter argument
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>the first Task that is matched by filter, default null</returns>

    public Task? Read(Func<Task, bool> filter)
    {
        Func<Task,bool> combined = t => filter(t) && IsActive(t); //make sure we only return active Tasks

        return DataSource.Tasks.FirstOrDefault(combined);
    }
}



