﻿using DO;
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
            finder(item.ID);
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
        Engineer? cur = finder(id);

        int index = DataSource.Engineers.IndexOf(cur); //get the index its in

        DataSource.Engineers[index] = cur with { Active = false };

    }

    /// <summary>
    /// Returns a reference to a Engineer object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Engineer if it exists, null otherwise</returns>
    public DO.Engineer? Read(int id)
    { 
        return finder(id);
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
        DO.Engineer cur = finder(item.ID);  //find the original

        int index = DataSource.Engineers.IndexOf(cur); //gets its index

        DataSource.Engineers[index] = item; //eliminate original add the new one

    }



    /// <summary>
    /// finds an Engineer based of its id, returns a reference to it, throws if the engineer does not exist
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>

    public Engineer finder(int id)
    {
        Engineer? cur = (DataSource.Engineers.Find(i => i.ID == id && i.Active));

        if (cur == null)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }

        return cur;
    }
}