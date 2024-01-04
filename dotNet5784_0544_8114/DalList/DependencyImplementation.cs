
using DO;
using DalApi;
namespace Dal;

internal class DependencyImplementation : IDependency


{
    /// <summary>
    /// Implementation of create function to add a Dependency to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new Dependency</returns>
    public int Create(DO.Dependency item)
    {
        int id = DataSource.Config.NextDependencyID;
        DO.Dependency _item = item with { ID = id };
        DataSource.Dependencies.Add(_item);
        return id;
    }

    public void Delete(int id)
    {
        int numRemoved = DataSource.Dependencies.RemoveAll(t => t.ID == id);
        if (numRemoved == 0)
        {
            throw new Exception("This ID does not exist");
        }
        if (numRemoved > 1)
        {
            throw new Exception("There is a much bigger problem ):");
        }
    }

    /// <summary>
    /// Returns a reference to a Dependency object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Dependency if it exists, null otherwise</returns>
    public DO.Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(t => t.ID == id);
    }

    /// <summary>
    /// Return a copy of all the Dependencies
    /// </summary>
    /// <returns></returns>
    public List<DO.Dependency> ReadAll()
    {
        return new List<DO.Dependency>(DataSource.Dependencies);
    }

    /// <summary>
    /// Update a Dependency 
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Dependency item)
    {
        Delete(item.ID);
        Create(item);
    }
}
