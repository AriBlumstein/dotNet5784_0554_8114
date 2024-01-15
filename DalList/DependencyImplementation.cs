


namespace Dal;

using DO;
using DalApi;

internal class DependencyImplementation : IDependency


{
    /// <summary>
    /// Implementation of create function to add a Dependency to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new Dependency</returns>
    public int Create(Dependency item)
    {
        //create it
        int id = DataSource.Config.NextDependencyID;

        Dependency _item = item with { ID = id };

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
    public Dependency Read(int id)
    {
        Dependency? cur = (DataSource.Dependencies.FirstOrDefault(i => i.ID == id && i.Active));

        if (cur == null)
        {
            throw new Exception($"Dependency with ID={id} does not exist");
        }

        return cur;
    }



    /// <summary>
    /// Update a Dependency 
    /// </summary>
    /// <param name="item"></param>
    public void Update(Dependency item)
    {
        Dependency cur = Read(item.ID);  //find the original

        int index = DataSource.Dependencies.IndexOf(cur); //gets its index

        DataSource.Dependencies[index] = item; //eliminate original, add new one

    }


    /// <summary>
    /// reset the database, set everything to inactive
    /// </summary>
    public void Reset()
    {
        DataSource.Dependencies.Clear();
    }


    /// <summary>
    /// Return a copy of all the Dependencies
    /// </summary>
    /// <returns></returns>
 
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        
        if(filter != null)
{
            return from item in DataSource.Dependencies
                   where filter(item) where isActive(item) //make sure to only return active items
                   select item;
        }
        return from item in DataSource.Dependencies
               where isActive(item) //make sure to only return active items
               select item;

    }


    /// <summary>
    /// returns if the entity in question is active
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>

    public bool isActive(Dependency d)
    {
        return d.Active;
    }

    /// <summary>
    /// read based on a filter argument
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.First(filter);
    }
}