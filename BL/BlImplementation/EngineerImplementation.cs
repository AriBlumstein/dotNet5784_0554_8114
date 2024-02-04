
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public BO.Engineer Create(BO.Engineer engineer)
    {
        validEngineer(engineer);

        //try adding it to the database
        try
        {
            _dal.Engineer.Create(new DO.Engineer { ID = engineer.ID, Name = engineer.Name, Exp = (Experience)engineer.Level, Cost = engineer.Cost, Email = engineer.Email });
        }
        catch (DalAlreadyExistsException e)
        {
            throw new BlAlreadyExistsException(e.Message, e);
        }

        return engineer;
    }

    

    public void Delete(BO.Engineer engineer)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(t => t.AssignedEngineer == engineer.ID && t.ActualEnd <= DateTime.Now); //here we have the tasks the engineer worked on previously

        if (tasks.Count() != 0) //there exists a task
        {
            throw new BlIllegalOperationException($"Engineer with ID {engineer.ID} cannot be deleted as he worked on tasks in the past");
        }

        // check if the engineer is currently working on a task
        if (engineer.Task != null)
        {
            throw new BlIllegalOperationException($"Engineer with ID {engineer.ID} is currently working on task {engineer.Task.ID}");
        }

        //try to delete
        try
        {
            _dal.Engineer.Delete(engineer.ID);
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

    }

    public BO.Engineer Read(int id)
    {
        DO.Engineer? engineer;

        try
        {
            engineer = _dal.Engineer.Read(id); //see if the engineer exists
        }
        catch(DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        return new BO.Engineer {ID=engineer.ID, Cost=engineer.Cost, Level=(EngineerExperience)engineer.Exp, Name=engineer.Name, Task=TaskSearcher(engineer.ID), Email=engineer.Email};
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool> filter = null)
    {
        IEnumerable<DO.Engineer?> engineers = _dal.Engineer.ReadAll(filter); //find the engineers that meet the condition

        IEnumerable<BO.Engineer> bEngineers = from engineer in engineers
                                              select new BO.Engineer
                                              {
                                                  ID = engineer.ID,
                                                  Name = engineer.Name,
                                                  Email=engineer.Email,
                                                  Level = (EngineerExperience)engineer.Exp, //this cast is lossless as the enums contain the same values
                                                  Cost = engineer.Cost,
                                                  Task = TaskSearcher(engineer.ID) //find the task the engineer is assigned, create TaskInEngineer instance
                                              };
        return bEngineers;
                                            
        
    }

    public BO.Engineer Update(BO.Engineer engineer)
    {
        validEngineer(engineer); 

        try
        {
            _dal.Engineer.Update(new DO.Engineer { ID = engineer.ID, Cost = engineer.Cost, Name = engineer.Name, Exp = (Experience)engineer.Level, Email = engineer.Email });
        }
        catch (DalDoesNotExistException ex) 
        { 
            throw new BlDoesNotExistException(ex.Message, ex);
        }

        return engineer;
    }


    
    public BO.TaskInEngineer TaskSearcher(int engineerId) 
    {
        //find the task that this engineer is assigned to 
        DO.Task? task=_dal.Task.Read(t => t.AssignedEngineer == engineerId && (t.ActualEnd==null||t.ActualEnd<=DateTime.Now)); //only find the tasks that have not yet been completed

        if (task != null)
            return new TaskInEngineer { ID = task.ID, Alias = task.Nickname };

        return null;
    }


    /// <summary>
    /// return true if the email address is a valid one
    /// </summary>
    /// <param name="email"></param>
    /// <returns>true if it is a valid email</returns>
    private bool isValidEmail(string email)
    {
        // Regular expression for a simple email validation
        string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        // Create a Regex object
        Regex regex = new Regex(pattern);

        // Use the IsMatch method to validate the email
        return regex.IsMatch(email);
    }


    /// <summary>
    /// method that checks the validity of an engineer, throw appropriate exception if not valid
    /// </summary>
    /// <param name="engineer"></param>
    private void validEngineer(BO.Engineer engineer)
    {
        //check validity of fields
        if (engineer.ID <= 0)
        {
            throw new BlIllegalPropertyException($"{engineer.ID} is not a valid id");
        }
        if (engineer.Name.Length == 0)
        {
            throw new BlIllegalPropertyException("An Engineer must have a non-empty name");
        }
        if (engineer.Cost <= 0)
        {
            throw new BlIllegalPropertyException($"{engineer.Cost} is an illegal salary");
        }
        if (!isValidEmail(engineer.Email))
        {
            throw new BlIllegalPropertyException($"{engineer.Email} is not a valid email");
        }
    }
}
