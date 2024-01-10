﻿


namespace DalApi;
using DO;

/// <summary>
/// interface for Dependency database
/// </summary>
public interface IDependency
{
    int Create(Dependency item); // Creates new entity object in DAL
    Dependency? Read(int id); // Reads entity object by its ID
    List<Dependency> ReadAll(); // stage 1 only, Reads all entity objects
    void Update(Dependency item); // Updates entity object
    void Delete(int id); // Deletes an object by its Id

    void Reset(); //reset the database

}
