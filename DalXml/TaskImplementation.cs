

using Dal;
using DalApi;
using DO;
using System.Xml.Linq;

namespace DalXml;

internal class TaskImplementation : ITask
{

    readonly string s_tasks_xml = "tasks";

    public int Create(DO.Task item)
    { 
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        DO.Task _task = item with { ID = Config.NextTaskId };
        tasks.Add(_task);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return item.ID;
    }

    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        DO.Task? t = tasks.Find(t => t.ID == id && isActive(t)); //engineer to delete
        if (t == null) throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        int index = tasks.IndexOf(t);
        tasks[index] = t with { Active = false };
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }

    public bool isActive(DO.Task item)
    {
        return item.Active;
    }

    public DO.Task? Read(int id)
    {
        IEnumerable<DO.Task> tasks = ReadAll()!;  //only returns the active members

        DO.Task? cur = tasks.FirstOrDefault(i => i.ID == id);

        if (cur == null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        }

        return cur;
    }

    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        return ReadAll().FirstOrDefault(filter!);
    }

    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (filter != null)
        {
            return from t in tasks
                   where filter(t)
                   where isActive(t) //make sure to only return active items
                   select t;
        }
        return from t in tasks
               where isActive(t) //make sure to only return active items
               select t;

    }

    public void Reset()
    {
        XMLTools.SaveListToXMLSerializer<DO.Task>(new List<DO.Task>(), s_tasks_xml);


        XElement configRoot = XMLTools.LoadListFromXMLElement("data-config");
        configRoot.Element("NextTaskID")!.Value = "1";
        XMLTools.SaveListToXMLElement(configRoot, "data-config");

    }

    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        DO.Task? toUpdate = tasks.FirstOrDefault(t => t.ID == item.ID && isActive(t));

        if (toUpdate == null)
            throw new DalDoesNotExistException($"Task with ID={item.ID} does not exist");

        int index = tasks.IndexOf(toUpdate); //gets its index

        tasks[index] = item; //eliminate original add the new one
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }
}
