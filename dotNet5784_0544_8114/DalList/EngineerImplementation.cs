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
        //if (DataSource.itemExists(item.ID, typeof(DO.Engineer))) throw new Exception($"Engineer with ID={item.ID} already exists");
        //DO.Engineer _item = item; //we do not want our user to point to the same reference anymore
        //DataSource.Engineers.Add(_item);
        //return item.ID;
        try
        {
            Read(item.ID);
            throw new Exception($"Engineer with ID={item.ID} already exists");
        }
        catch (Exception ex)
        {
            DO.Engineer _item = item; //we do not want our user to point to the same reference anymore
            DataSource.Engineers.Add(_item);
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
    /// /// <exception cref="Exception"></exception>
    public DO.Engineer Read(int id)
    {
    
        Engineer? cur = (DataSource.Engineers.Find(i => i.ID == id && i.Active));

        if (cur == null)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }

            return cur;
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
        DO.Engineer cur = Read(item.ID);  //find the original

        int index = DataSource.Engineers.IndexOf(cur); //gets its index

        DataSource.Engineers[index] = item; //eliminate original add the new one

    }

    /// <summary>
    /// reset the database, set everything to inactive
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < DataSource.Engineers.Count; ++i)
        {
            DataSource.Engineers[i] = DataSource.Engineers[i] with { Active = false };
        }
    }



}