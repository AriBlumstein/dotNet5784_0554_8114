namespace Dal;

using DO;
using DalApi;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// create a new engineer
    /// </summary>
    /// <param name="item"></param>
    /// <returns>the id of the created engineer</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        //if (DataSource.itemExists(item.ID, typeof(DO.Engineer))) throw new Exception($"Engineer with ID={item.ID} already exists");
        //DO.Engineer _item = item; //we do not want our user to point to the same reference anymore
        //DataSource.Engineers.Add(_item);
        //return item.ID;
        try
        {
            Read(item.ID);
            throw new DalAlreadyExistsException($"Engineer with ID={item.ID} already exists");
        }
        catch (Exception ex)
        {
            
            DataSource.Engineers.Add(item);
            return item.ID;

        }
    }

    /// <summary>
    /// deletes an Engineer given its id
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        Engineer cur = Read(id);

        int index = DataSource.Engineers.IndexOf(cur); //get the index its in

        DataSource.Engineers[index] = cur with { Active = false };

    }

    /// <summary>
    /// Returns a reference to a Engineer object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Engineer if it exists, null otherwise</returns>
    /// /// <exception cref="DalDoesNotExistException"></exception>
    public Engineer Read(int id)
    {

        Engineer? cur = DataSource.Engineers.FirstOrDefault(i => i.ID == id && i.Active);

        if (cur == null)
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        }

            return cur;
    }

  

    /// <summary>
    /// Update a Engineer 
    /// </summary>
    /// <param name="item"></param>
    public void Update(Engineer item)
    {
        Engineer cur = Read(item.ID);  //find the original

        int index = DataSource.Engineers.IndexOf(cur); //gets its index

        DataSource.Engineers[index] = item; //eliminate original add the new one

    }

    /// <summary>
    /// reset the database, set everything to inactive
    /// </summary>
    public void Reset()
    {
        DataSource.Engineers.Clear();
    }

    /// <summary>
    /// Return a copy of all the Engineers
    /// </summary>
    /// <returns>an IEnumerable of Engineers</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   where isActive(item) //make sure to only return active items
                   select item;
        }
        return from item in DataSource.Engineers
               where isActive(item) //make sure to only return active items
               select item;

    }


    /// <summary>
    /// returns if the entity in question is active
    /// </summary>
    /// <param name="e"></param>
    /// <returns>true if the Engineer is active</returns>

    public bool isActive(Engineer e)
    {
        return e.Active;
    }

    /// <summary>
    /// read based on a filter argument
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>the first engineer that is matched by the filter, null by default</returns>
    
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        Func<Engineer, bool> combined = e => filter(e) && isActive(e); //make sure we only return active Engineers
        return DataSource.Engineers.FirstOrDefault(combined);
    }
}


