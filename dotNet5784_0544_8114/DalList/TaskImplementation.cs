


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
        
        Task _item = item with { ID = id };

        DataSource.Tasks.Add(_item);
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
    ///  /// <exception cref="Exception"></exception>
    public Task Read(int id)
    {
        Task? cur = (DataSource.Tasks.FirstOrDefault(i => i.ID == id && i.Active));

        if (cur == null)
        {
            throw new Exception($"Task with ID={id} does not exist");
        }

        return cur;
    }

    /// <summary>
    /// Return a copy of all the Tasks
    /// </summary>
    /// <returns></returns>
 
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   where isActive(item) //make sure to only return active items
                   select item;
        }
        return from item in DataSource.Tasks
               where isActive(item)  //make sure to only return active items
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
    /// reset the database, set everything to inactive
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
    public bool isActive(Task t)
    {
        return t.Active;
    }



}
