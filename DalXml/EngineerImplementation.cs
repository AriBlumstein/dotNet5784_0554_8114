

namespace DalXml;

using Dal;
using DalApi;
using DO;


internal class EngineerImplementation : IEngineer
{

    readonly string s_engineers_xml = "engineers";
    public int Create(Engineer item)
    {
        List<Engineer> engineers = ReadAll().ToList()!;
        if (engineers.Exists(e=>e.ID == item.ID && isActive(e)))
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.ID} already exists");
        }
      

        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);
        return item.ID;
    }

    public void Delete(int id)
    {
        List<Engineer> engineers = ReadAll().ToList()!;

        Engineer? e = engineers.Find(e=>e.ID==id && isActive(e)) ; //engineer to delete
        if (e==null) throw new DalDoesNotExistException($"Engineer with ID={e.ID} does not exist");
        int index = engineers.IndexOf(e);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);
    }

    public bool isActive(Engineer e)
    {
        return e.Active;
    }

    public Engineer Read(int id)
    {
        List<Engineer> engineers = ReadAll().ToList()!;
        Engineer? cur = engineers.FirstOrDefault(i => i.ID == id && i.Active);

        if (cur == null)
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        }

        return cur;


    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        Func<Engineer, bool> combined = e => filter(e) && isActive(e); //make sure we only return active Engineers
        return ReadAll().FirstOrDefault(combined!);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter != null)
        {
            return from e in engineers
                   where filter(e)
                   where isActive(e) //make sure to only return active items
                   select e;
        }
        return from e in engineers
               where isActive(e) //make sure to only return active items
               select e;

    }

    public void Reset()
    {
        XMLTools.SaveListToXMLSerializer<Engineer>(new List<Engineer>(), s_engineers_xml);
    }

    public void Update(Engineer cur)
    {
        List<Engineer> engineers = ReadAll().ToList()!;

        int index = engineers.IndexOf(cur); //gets its index
        if (index == -1) throw new DalDoesNotExistException($"Engineer with ID={cur.ID} does not exist");

        engineers[index] = cur; //eliminate original add the new one
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);
    }
}
