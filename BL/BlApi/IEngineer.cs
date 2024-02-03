

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
    BO.Engineer Create(BO.Engineer engineer);


    /// <summary>
    /// delete an engineer, admin view
    /// </summary>
    /// <param name="engineer"></param>
    void Delete(BO.Engineer engineer);

    /// <summary>
    /// update an Engineer, admin view
    /// </summary>
    /// <param name="engineer"></param>
    BO.Engineer Update(BO.Engineer engineer);

    /// <summary>
    /// method that returns the task the engineer is working on at the moment
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    BO.TaskInEngineer TaskSearcher(int id);


    /// <summary>
    /// return true if the email address is a valid one
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public bool IsValidEmail(string email);


    /// <summary>
    /// method that checks the validity of an engineer, throw appropriate exception if not valid
    /// </summary>
    /// <param name="engineer"></param>
    public void ValidEngineer(BO.Engineer engineer);


}
