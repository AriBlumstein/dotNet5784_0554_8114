﻿

namespace BlApi;


/// <summary>
/// Engineer Interface
/// </summary>
public interface IEngineer
{

    /// <summary>
    /// return a list of engineers based on the filter function, for admin view
    /// </summary>
    /// <param name="filter"></param>
    /// <returns><IEnumerable<BO.Engineer></returns>
    IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool> filter=null);

    /// <summary>
    /// return an engineer given the id for engineer view
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.Engineer</returns>
    BO.Engineer Read(int id);

    /// <summary>
    /// creates a new engineer for the dal, for admin view
    /// </summary>
    /// <param name="engineer"></param>
    /// <returns>the id of the engineer created</returns>
    int Create(BO.Engineer engineer);


    /// <summary>
    /// delete an engineer, admin view
    /// </summary>
    /// <param name="engineer"></param>
    void Delete(BO.Engineer engineer);

    /// <summary>
    /// update an Engineer, admin view
    /// </summary>
    /// <param name="engineer"></param>
    void update(BO.Engineer engineer);


}
