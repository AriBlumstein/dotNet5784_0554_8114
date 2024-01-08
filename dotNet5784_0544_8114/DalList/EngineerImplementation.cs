using DO;
using DalApi;

namespace Dal;

public class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Implementation of create function to add a Engineer to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new Engineer</returns>
    public int Create(DO.Engineer item)
    {
        if (DataSource.itemExists(item.ID, typeof(DO.Engineer))) throw new Exception($"Engineer with ID={item.ID} already exists");
        DO.Engineer _item = item; //we do not want our user to point to the same refernce anymore
        DataSource.Engineers.Add(_item);
        return item.ID;
    }

    public void Delete(int id)
    {
        Engineer? cur = (DataSource.Engineers.Find(i => i.ID == id));

        if (cur == null)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }

        int index = DataSource.Engineers.IndexOf(cur);
        DataSource.Engineers[index] = cur with { Active = false };
    }

    /// <summary>
    /// Returns a reference to a Engineer object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Engineer if it exists, null otherwise</returns>
    public DO.Engineer? Read(int id)
    {
        if (!DataSource.itemExists(id, typeof(DO.Engineer))) throw new Exception($"Engineer with ID={id} does not exist");
        return DataSource.Engineers.Find(t => t.ID == id);
    }

    /// <summary>
    /// Return a copy of all the Engineers
    /// </summary>
    /// <returns></returns>
    public List<DO.Engineer> ReadAll()
    {
        return DataSource.Engineers.FindAll(i => i.Active == true);
    }

    /// <summary>
    /// Update a Engineer 
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Engineer item)
    {
        Delete(item.ID);
        Create(item);
    }
}