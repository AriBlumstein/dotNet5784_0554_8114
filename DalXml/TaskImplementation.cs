

using Dal;


namespace DalXml;

using DalApi;
using DO;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{

    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// Implementation of create function to add a task to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new task</returns>
    public int Create(Task item)
    { 
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        DO.Task _task = item with { ID = Config.NextTaskId };
        tasks.Add(_task);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, s_tasks_xml);
        return _task.ID;
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        Task? t = tasks.Find(t => t.ID == id && IsActive(t)); //engineer to delete
        if (t == null) throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        int index = tasks.IndexOf(t);
        tasks[index] = t with { Active = false };
        XMLTools.SaveListToXMLSerializer<Task>(tasks, s_tasks_xml);
    }

    /// <summary>
    /// returns if the entity in question is active
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool IsActive(Task item)
    {
        return item.Active;
    }

    /// <summary>
    /// Returns a reference to a Task object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the task if it exists, null otherwise</returns>
    ///  /// <exception cref="DalDoesNotExistException"></exception>
    public Task? Read(int id)
    {
        IEnumerable<Task> tasks = ReadAll()!;  //only returns the active members

        Task? cur = tasks.FirstOrDefault(i => i.ID == id);

        return cur ?? throw new DalDoesNotExistException($"Task with ID={id} does not exist");

 
    }

    /// <summary>
    /// read based on a filter argument
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>the first Task that is matched by filter, default null</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return ReadAll().FirstOrDefault(filter!);
    }

    /// <summary>
    /// Return a copy of all the Tasks
    /// </summary>
    /// <returns>An IEnumerable of tasks</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter != null)
        {
            return from t in tasks
                   where filter(t)
                   where IsActive(t) //make sure to only return active items
                   select t;
        }
        return from t in tasks
               where IsActive(t) //make sure to only return active items
               select t;

    }

    /// <summary>
    /// reset the tasks
    /// </summary>
    public void Reset()
    {
        XMLTools.SaveListToXMLSerializer<Task>(null, s_tasks_xml);


        XElement configRoot = XMLTools.LoadListFromXMLElement("data-config");
        configRoot.Element("NextTaskID")!.Value = "1";
        XMLTools.SaveListToXMLElement(configRoot, "data-config");

    }

    /// <summary>
    /// Update a task 
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        DO.Task? toUpdate = tasks.FirstOrDefault(t => t.ID == item.ID && IsActive(t));

        if (toUpdate == null)
            throw new DalDoesNotExistException($"Task with ID={item.ID} does not exist");

        int index = tasks.IndexOf(toUpdate); //gets its index

        tasks[index] = item; //eliminate original add the new one
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }
}
