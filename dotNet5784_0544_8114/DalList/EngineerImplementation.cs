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
        DO.Engineer _item = item;
        DataSource.Engineers.Add(_item);
        return item.ID;
    }

    public void Delete(int id)
    {
        int numRemoved = DataSource.Engineers.RemoveAll(t => t.ID == id);
        if (numRemoved == 0)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }
        if (numRemoved > 1)
        {
            throw new Exception("There is a much bigger problem ):");
        }
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
        return new List<DO.Engineer>(DataSource.Engineers);
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