
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
        if (checkCircularDependency(item))
        {
            throw new Exception("This item causes a circular dependency");
        }
        DataSource.Dependencies.Add(_item);
        
        return id;
    }

    public void Delete(int id)
    {
        int numRemoved = DataSource.Dependencies.RemoveAll(t => t.ID == id);
        if (numRemoved == 0)
        {
            throw new Exception($"Dependency with ID={id} does not exist");
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
        if (!DataSource.itemExists(id, typeof(DO.Dependency))) throw new Exception($"Dependency with ID={id} does not exist");
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item">the new dependency to check if it creates a circular dependency</param>
    /// <returns>true if circular dependency, false otherwise</returns>
    public bool checkCircularDependency(DO.Dependency item)
    {
        List<DO.Dependency> dependencies = DataSource.Dependencies;
        bool checkCircularHelper(DO.Dependency item, int dependentID)
        {
            List<DO.Dependency> chain = new List<DO.Dependency>();
            bool res;
            chain = dependencies.FindAll(d => d.DependentID == item.RequisiteID);
            foreach (var d in chain)
            {
                if (d.RequisiteID == dependentID)
                    return true;
                 res=checkCircularHelper(d,dependentID);
                if (res) return res;
            }
            return false;
        }
        return checkCircularHelper(item, item.RequisiteID);
        
          }
}
