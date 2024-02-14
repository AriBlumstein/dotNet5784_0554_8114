
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;

internal class EngineerImplementation : IEngineer
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

        //add the task if we can
        if (engineer.Task != null)
        {
            DO.Task task = _dal.Task.Read(engineer.Task.ID) with { AssignedEngineer = engineer.ID };
            _dal.Task.Update(task);
        }

        return engineer;
    }

    

    public void Delete(int id)
    {
        BO.Engineer engineer;
        try {
          engineer  = Read(id);
        }
        catch (DalDoesNotExistException e) { throw new BlDoesNotExistException(e.Message, e); }

        //check if there are no completed tasks
        IEnumerable<DO.Task?> completedTasks = from task in _dal.Task.ReadAll(t => t.AssignedEngineer == engineer.ID)
                                               where task.ActualEnd<=DateTime.Now
                                               select task;  // return the completed tasks

       
        
        if (completedTasks.Count() != 0) //there exists a task
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

    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        IEnumerable<DO.Engineer?> engineers = _dal.Engineer.ReadAll(filter); //find the engineers that meet the condition

        IEnumerable<BO.Engineer> bEngineers = from engineer in engineers
                                              let assignedTask = TaskSearcher(engineer.ID)
                                              select new BO.Engineer
                                              {
                                                  ID = engineer.ID,
                                                  Name = engineer.Name,
                                                  Email=engineer.Email,
                                                  Level = (EngineerExperience)engineer.Exp, //this cast is lossless as the enums contain the same values
                                                  Cost = engineer.Cost,
                                                  Task = assignedTask 
                                              };
        return bEngineers;
                                            
        
    }

    public BO.Engineer Update(BO.Engineer engineer)
    {
        validEngineer(engineer);

        BO.Engineer oldEngineer = Read(engineer.ID);

        try
        {
            _dal.Engineer.Update(new DO.Engineer { ID = engineer.ID, Cost = engineer.Cost, Name = engineer.Name, Exp = (Experience)engineer.Level, Email = engineer.Email });
        }
        catch (DalDoesNotExistException ex) 
        { 
            throw new BlDoesNotExistException(ex.Message, ex);
        }

        if (engineer.Task != null && (oldEngineer.Task==null || oldEngineer.Task.ID!=engineer.Task.ID)) //if we have a new task and we didn't have one before or we had a different one before
        {
            //add the engineer to the task
            try
            {
                DO.Task newTask = _dal.Task.Read(engineer.Task.ID) with { AssignedEngineer = engineer.ID };
                _dal.Task.Update(newTask);
                
            }
            catch (DalDoesNotExistException ex)
            {
                throw new BlDoesNotExistException(ex.Message, ex);
            }

        }

        DO.Task oldTask;

        //unassign the old task if it is different 
        if (oldEngineer.Task != null && ( engineer.Task==null || oldEngineer.Task.ID!=engineer.Task.ID)) //if we had an old task, adn we don't have one now or we have a different one now
        { 

            oldTask = _dal.Task.Read(oldEngineer.Task.ID) with { AssignedEngineer = null };
            _dal.Task.Update(oldTask);
        }


        return Read(engineer.ID);
    }

    public IEnumerable<IGrouping<BO.EngineerExperience, BO.Engineer>> ReadGroupsOfExperience()
    {
        return from item in _dal.Engineer.ReadAll()
               let bEngineer = new BO.Engineer
               {
                   ID = item.ID,
                   Name = item.Name,
                   Email = item.Email,
                   Level = (BO.EngineerExperience)item.Exp,
                   Task = TaskSearcher(item.ID),
                   Cost = item.Cost

               }
               group bEngineer by bEngineer.Level into res
               orderby res.Key
               select res;  
    }

    public BO.TaskInEngineer? TaskSearcher(int engineerId) 
    {
        //find the task that this engineer is assigned to 
        DO.Task? task=_dal.Task.Read(t => t.AssignedEngineer == engineerId && (t.ActualEnd==null||t.ActualEnd>=DateTime.Now)); //only find the tasks that have not yet been completed, if completed, I do not want it to show up as the task the engineer is assigned to

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
        if(engineer.Name==null)
        {
            throw new BlNullPropertyException("Name of engineer cannot be null");
        }
        if (engineer.Name.Length == 0)
        {
            throw new BlIllegalPropertyException("An Engineer must have a non-empty name");
        }
        if (engineer.Cost <= 0)
        {
            throw new BlIllegalPropertyException($"{engineer.Cost} is an illegal salary");
        }
        if (engineer.Email == null)
        {
            throw new BlNullPropertyException("Email cannot be null");
        }
        if (!isValidEmail(engineer.Email))
        {
            throw new BlIllegalPropertyException($"{engineer.Email} is not a valid email");
        }
        if (engineer.Task != null) {

            //check if we are in production, if we aren't in production, we cannot assign an engineer

            try
            {
                _dal.Config.GetProjectStart();
            }
            catch(Exception ) 
            {
                throw new BlIllegalOperationException("Cannot assign tasks to engineers before production");
            }

            DO.Task? task = _dal.Task.Read(t => t.ID == engineer.Task.ID);

            if (task==null)
            {
                throw new BlIllegalPropertyException($"Task {engineer.Task.ID} does not exist");
            } 

            if(task.AssignedEngineer!=null && task.AssignedEngineer != engineer.ID)
            {
                throw new BlIllegalOperationException($"task {task.ID} is already assigned to engineer {task.AssignedEngineer}");
            }

            if (task.Difficulty>(Experience)engineer.Level)
            {
                throw new BlIllegalOperationException($"task {task.ID} is too hard for engineer {engineer.ID}");
            }



        }
    }
}
