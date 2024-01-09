
using DO;
using DalApi;


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
        //create it
        int id = DataSource.Config.NextDependencyID;

        DO.Dependency _item = item with { ID = id };

        DataSource.Dependencies.Add(_item);

        return id;
    }

    /// <summary>
    /// deletes a dependency
    /// </summary>
    /// <param name="id"></param>

    public void Delete(int id)
    {
        Dependency cur = Read(id);

        int index = DataSource.Dependencies.IndexOf(cur);

        DataSource.Dependencies[index] = cur with { Active = false };

    }



    /// <summary>
    /// Returns a reference to a Dependency object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Dependency if it exists, null otherwise</returns>
    /// /// <exception cref="Exception"></exception>
    public DO.Dependency Read(int id)
    {
        Dependency? cur = (DataSource.Dependencies.Find(i => i.ID == id && i.Active));

        if (cur == null)
        {
            throw new Exception($"Dependency with ID={id} does not exist");
        }

        return cur;
    }

    /// <summary>
    /// Return a copy of all the Dependencies
    /// </summary>
    /// <returns></returns>
    public List<DO.Dependency> ReadAll()
    {
        return DataSource.Dependencies.FindAll(i => i.Active);
    }

    /// <summary>
    /// Update a Dependency 
    /// </summary>
    /// <param name="item"></param>
    public void Update(DO.Dependency item)
    {
        DO.Dependency cur = Read(item.ID);  //find the original

        int index = DataSource.Dependencies.IndexOf(cur); //gets its index

        DataSource.Dependencies[index] = item; //eliminate original, add new one

    }



}