
using DO;
using DalApi;

namespace Dal;

public class TaskImplementation : ITask
{
    /// <summary>
    /// Implementation of create function to add a task to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new task</returns>
    public int Create(DO.Task item)
    {
        int id = DataSource.Config.NextTaskID;
        
        DO.Task _item = item with { ID = id };

        DataSource.Tasks.Add(_item);
        return id;
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id"></param>

    public void Delete(int id)
    {
        DO.Task? cur = finder(id);

        int index = DataSource.Tasks.IndexOf(cur);

        DataSource.Tasks[index] = cur with { Active = false };
    }

  

    /// <summary>
    /// Returns a reference to a Task object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the task if it exists, null otherwise</returns>
    public DO.Task? Read(int id)
    {
        return finder(id);
    }

    /// <summary>
    /// Return a copy of all the Tasks
    /// </summary>
    /// <returns></returns>
    public List<DO.Task> ReadAll()
    {
        return DataSource.Tasks.FindAll(i => i.Active);
    }
    
    /// <summary>
    /// Update a task 
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Task item)
    {
        DO.Task cur = finder(item.ID); //find the original item

        int index = DataSource.Tasks.IndexOf(cur); //find its index

        DataSource.Tasks[index] = item; //eliminate original, add new one
    }


    /// <summary>
    /// see if tasks exists by checking its id and returns its reference, throws if it doesn't exist
    /// </summary>
    /// <param name="id"></param>
    /// <returns>a reference if it exists</returns>
    /// <exception cref="Exception"></exception>
    public DO.Task finder(int id)
    {
        DO.Task? cur = (DataSource.Tasks.Find(i => i.ID == id && i.Active));

        if (cur == null)
        {
            throw new Exception($"Task with ID={id} does not exist");
        }

        return cur;
    }
}
