
using DO;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal;

public class DependencyImplementation : IDependency


{
    /// <summary>
    /// Implementation of create function to add a Dependency to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new Dependency</returns>
    public int Create(DO.Dependency item)
    {
        if (ReadAll().FindAll(i => (i.DependentID == item.DependentID && i.RequisiteID == item.RequisiteID && i.Active)).Count() > 0)
            throw new Exception($"Dependency where {item.DependentID} is dependent on {item.RequisiteID} already exists");
        
        if (checkCircularDependency(item))
        {
            throw new Exception("This item causes a circular dependency");
        }

        int id = DataSource.Config.NextDependencyID;
        DO.Dependency _item = item with { ID = id };

        DataSource.Dependencies.Add(_item);
        
        return id;
    }

    /// <summary>
    /// deletes a dependency
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>

    public void Delete(int id)
    {

        Dependency? cur=(DataSource.Dependencies.Find(i=> i.ID == id));
        
        if (cur==null)
        {
            throw new Exception($"Dependency with ID={id} does not exist");
        }

        int index = DataSource.Dependencies.IndexOf(cur);
        DataSource.Dependencies[index] = cur with { Active = false };
        

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
        return DataSource.Dependencies.FindAll(i=> i.Active);
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

        if (item.RequisiteID==item.DependentID)
        {
            return true;
        }

        bool checkCircularHelper(DO.Dependency item, int dependentID)
        {
            List<DO.Dependency> chain = new List<DO.Dependency>();
            bool res;
            chain = DataSource.Dependencies.FindAll(i => i.DependentID == item.RequisiteID && i.Active);
            foreach (var d in chain)
            {
                if (d.RequisiteID == dependentID)
                    return true;
                 res=checkCircularHelper(d,dependentID);
                if (res) return res;
            }
            return false;
        }
        return checkCircularHelper(item, item.DependentID);
        
          }
}
