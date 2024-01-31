

namespace DalXml;

using Dal;
using DalApi;
using DO;


internal class EngineerImplementation : IEngineer
{

    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// create a new engineer
    /// </summary>
    /// <param name="item"></param>
    /// <returns>the id of the created engineer</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (engineers.Exists(e=>e.ID == item.ID && IsActive(e)))
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.ID} already exists");
        }
      

        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);
        return item.ID;
    }

    /// <summary>
    /// deletes an Engineer given its id
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        Engineer? e = engineers.Find(e=>e.ID==id && IsActive(e)) ; //engineer to delete
        if (e==null) throw new DalDoesNotExistException($"Engineer with ID={e.ID} does not exist");
        int index = engineers.IndexOf(e);
        engineers[index] = e with { Active = false };
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);
    }



    /// <summary>
    /// returns if the entity in question is active
    /// </summary>
    /// <param name="e"></param>
    /// <returns>true if the Engineer is active</returns>
    public bool IsActive(Engineer e)
    {
        return e.Active;
    }


    /// <summary>
    /// Returns a reference to a Engineer object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Engineer if it exists, null otherwise</returns>
    /// /// <exception cref="DalDoesNotExistException"></exception>
    public Engineer Read(int id)
    {
        IEnumerable<Engineer> engineers = ReadAll()!;  //only returns the active members
        
        Engineer? cur = engineers.FirstOrDefault(i => i.ID == id); 

        return cur ?? throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");


    }

    /// <summary>
    /// read based on a filter argument
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>the first engineer that is matched by the filter, null by default</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return ReadAll().FirstOrDefault(filter!);
    }


    /// <summary>
    /// Return a copy of all the Engineers
    /// </summary>
    /// <returns>an IEnumerable of Engineers</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter != null)
        {
            return from e in engineers
                   where filter(e)
                   where IsActive(e) //make sure to only return active items
                   select e;
        }
        return from e in engineers
               where IsActive(e) //make sure to only return active items
               select e;

    }

    /// <summary>
    /// reset the engineers
    /// </summary>
    public void Reset()
    {
        XMLTools.SaveListToXMLSerializer<Engineer>(null, s_engineers_xml);
    }


    /// <summary>
    /// Update a Engineer 
    /// </summary>
    /// <param name="item"></param>
    public void Update(Engineer cur)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        Engineer? toUpdate=engineers.FirstOrDefault(e=>e.ID==cur.ID&&IsActive(e));

        if (toUpdate == null)
            throw new DalDoesNotExistException($"Engineer with ID={cur.ID} does not exist");

        int index = engineers.IndexOf(toUpdate); //gets its index
        
        engineers[index] = cur; //eliminate original add the new one
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);
    }
}
