
namespace BlImplementation;


using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    //helper private class
    private class twoDate
    {
        public DateTime? Start { get; init; }
        public DateTime? End { get; init; }
    }

    public BO.Task Create(BO.Task task)
    {
        //see if we are in production
        try
        {
            _dal.Config.GetProjectStart();
            throw new BlIllegalOperationException("Cannot add a task during production");
        }
        catch(Exception)
        {

        }

        //we can now add a task
        validateTask(task); //check the the task is valid

        //try adding it to the database
        try
        {

            _dal.Task.Create(getDOTask(task)) ;
        }
        catch (DalAlreadyExistsException e)
        {
            throw new BlAlreadyExistsException(e.Message, e);
        }


        //add all the dependencies, we can as validTask checks for the cicular dependency problem
        IEnumerable<DO.Dependency> dependencies=task.Dependencies.Select(t=>new DO.Dependency { DependentID = task.ID, RequisiteID = t.ID });

        foreach (DO.Dependency dep in dependencies)
        {
            _dal.Dependency.Create(dep);
        }

        return task;
    }
    public void Delete(int id)
    {
       //see if we are in production
        try
        {
           _dal.Config.GetProjectStart();
            
           throw new BlIllegalOperationException("cannot delete task during production");
            
        }
        catch (Exception) { }

        //check that it isn't a requisite for other tasks
        IEnumerable<DO.Dependency?> depdencies= _dal.Dependency.ReadAll(d=>d.RequisiteID == id);

        if(depdencies.Count()!=0) 
        {
            throw new BlIllegalOperationException($"Cannot delete task {id} as {depdencies.Count()} tasks are dependent on it ");

        }

        //try to delete

        try
        {
            _dal.Task.Delete(id);
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

    }
    public BO.Task Read(int id)
    {
        DO.Task dTask;
        try
        {
            dTask = _dal.Task.Read(id)!;
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        return getBOTask(dTask);

    }

    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
       return _dal.Task.ReadAll(filter).Select(t=>getBOTask(t));
    }


    public BO.Task Update(BO.Task task)
    {
        //see if we are in production
        bool production = false;
        try
        {
            _dal.Config.GetProjectStart();
            production = true;
        }
        catch (Exception) { }

        //if we are not in production we can update the dates
        if (!production)
        {
            validateTask(task);
        }
        else  //we cannot update the dates
        {
            validateTask(task); validateTaskProduction(task);
        }
        
        DO.Task dTask = getDOTask(task);


        //try to update
        try
        {
            _dal.Task.Update(dTask);
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        return task;

    }

   
    public BO.Task UpdateProjectedStartDate(int id, DateTime startDate)
    {
        DO.Task dTask;
        try
        {
            dTask = _dal.Task.Read(id);
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        //get the dependencies
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d=>d.DependentID == id);

        IEnumerable<twoDate> dependenciesTimes;

        try
        {
            //get the requisite tasks dates
            dependenciesTimes = from dep in dependencies
                                let rTask=_dal.Task.Read(dep.RequisiteID) //read the task that we are dependent on
                                select new twoDate { Start = rTask.ProjectedStart, End = rTask.ActualEnd }; //get the dates we need to check
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        //check that the times for any requisite task are well defined
        foreach (var dep in dependenciesTimes)
        {
            if (!dep.Start.HasValue || !dep.End.HasValue || startDate < dep.End)
            {
                throw new BlIllegalOperationException($"Cannot update task {id}'s start date");
            }
        }


        //now we can do the update
        BO.Task bTask = getBOTask(dTask);


        return this.Update(bTask); // does the production checks, and returns the object itself
    }


    /// <summary>
    /// private method that helps us get the status of a task
    /// </summary>
    /// <param name="task"></param>
    /// <returns>Status?</returns>
    private Status? getStatus(DO.Task task)
    {
        if (task.ProjectedStart == null) return Status.Unscheduled;
        else if (task.ProjectedStart >= DateTime.Now) return Status.Scheduled;
        else if (task.ActualEnd == null) return Status.OnTrack;
        else if (task.ActualEnd <= DateTime.Now) return Status.Completed;
        return null;
    }


    /// <summary>
    /// helper method to get the dependencies of a task
    /// </summary>
    /// <param name="taskID"></param>
    /// <returns></returns>
    private List<TaskInList> getDependencies(int taskID)
    {
        IEnumerable<TaskInList> dependecies = _dal.Dependency.ReadAll(i=>i.DependentID==taskID).Select(i=>i.RequisiteID).Select(id=>getTaskInList(id));

        return dependecies.ToList();

      
    }

    //helper method to get the assigned engineer
    private BO.EngineerInTask? getEngineer(int? engineerID)
    {
        if (engineerID == null) return null;
        try
        {
            DO.Engineer? engineer = _dal.Engineer.Read(engineerID.Value);
            return new BO.EngineerInTask
            {
                ID = engineerID.Value,
                Name = engineer!.Name,
            };
        }
        catch (DalDoesNotExistException e) { throw new BlDoesNotExistException(e.Message, e); }
    }

    /// <summary>
    /// helper method to create a TaskInList
    /// </summary>
    /// <param name="taskID"></param>
    /// <returns>TaskInList</returns>
    /// <exception cref="BlDoesNotExistException"></exception>

    private BO.TaskInList getTaskInList(int taskID)
    {
        try { 
        DO.Task task = _dal.Task.Read(taskID)!;
            return new BO.TaskInList
            {
                ID = task.ID,
                Description = task.Description,
                Name=task.Nickname,
                Status=getStatus(task),
            };
        }
        catch (DalDoesNotExistException e) { throw new BlDoesNotExistException(e.Message, e);
        }
        
        
    }


  /// <summary>
  /// helper method that calculates projectedEnd
  /// </summary>
  /// <param name="task"></param>
  /// <returns>DateTime?</returns>
    private DateTime? getProjectedEnd(DO.Task task)
    {
        bool projectedStartNull = !task.ProjectedStart.HasValue;
        bool actualStartDateNull = !task.ActualStart.HasValue;
        bool durationNull = !task.Duration.HasValue;
        if (projectedStartNull && actualStartDateNull && durationNull) return null;
        if (!projectedStartNull && !durationNull && actualStartDateNull) return task.ProjectedStart!.Value.AddDays(task.Duration!.Value);
        if (!actualStartDateNull && !durationNull && projectedStartNull) return task.ActualStart!.Value.AddDays(task.Duration!.Value);
        //Both exist and we have to take the max
        if (task.ProjectedStart!.Value.AddDays(task.Duration!.Value) >= task.ActualStart!.Value.AddDays(task.Duration!.Value))
        {
            return task.ProjectedStart!.Value.AddDays(task.Duration!.Value);
        }
        return task.ActualStart!.Value.AddDays(task.Duration!.Value);
    }


    /// <summary>
    /// helper method that calculates duration
    /// </summary>
    /// <param name="task"></param>
    /// <returns>duration?</returns>
    private int? getDurationDays(BO.Task task)
    {
        bool actualStartNull = !task.ActualStart.HasValue;
        bool projectedStartNull = !task.ProjectedStart.HasValue;
        bool projectedEndNull = !task.ProjectedEnd.HasValue;
        if ((actualStartNull && projectedStartNull) || projectedEndNull) return null;
        if (!projectedStartNull && actualStartNull) return (task.ProjectedEnd!.Value - task.ProjectedStart!.Value).Days;
        if (projectedStartNull && !actualStartNull) return (task.ProjectedEnd!.Value - task.ActualStart!.Value).Days;
        if ((task.ProjectedEnd!.Value - task.ActualStart!.Value).Days >= (task.ProjectedEnd!.Value - task.ProjectedStart!.Value).Days)
        {
            return (task.ProjectedEnd!.Value - task.ActualStart!.Value).Days;
        }
        return (task.ProjectedEnd!.Value - task.ProjectedStart!.Value).Days;
    }

    /// <summary>
    /// valdidate the properties of a task
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BlIllegalPropertyException"></exception>
    /// <exception cref="BlNullPropertyException"></exception>

    private void validateTask(BO.Task task)
    {
        if(task.ID<=0)
        {
            throw new BlIllegalPropertyException("ID must be positive");
        }
        if(task.Name==null)
        {
            throw new BlNullPropertyException("Must not be a null name");
        }
        if(task.Name.Length==0)
        {
            throw new BlIllegalPropertyException("Name must be none empty");
        }

        //check that the engineer exists

        if(task.Engineer!=null)
        {
            try
            {
                _dal.Engineer.Read(task.Engineer.ID);
            }
            catch (DalDoesNotExistException ex) 
            {
                throw new BlDoesNotExistException(ex.Message,ex)
            }
        }

        //we will now check for a circular dependency and that the tasks we want to make as a dependency exist
        IEnumerable<DO.Dependency> dependencies=task.Dependencies.Select(t=>new DO.Dependency {DependentID=task.ID, RequisiteID=t.ID});
        foreach(DO.Dependency dep in dependencies)
        {
            try
            {
                _dal.Task.Read(dep.RequisiteID);
            }
            catch (DalDoesNotExistException e)
            {
                throw new BlDoesNotExistException(e.Message, e); //the task we tried to to make as a dependency did not exist
            }
            
            if(checkCircularDependency(dep))
            {
                throw new BlIllegalPropertyException("Cannot creat a circular dependency");
            }
        }


    }

    /// <summary>
    /// method checks if we insert a dependecy will we have a circular dependency because of it
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true if there would be a circular dependency</returns>
    private bool checkCircularDependency(DO.Dependency item)
    {

        if (item.RequisiteID == item.DependentID)
        {
            return true;            // simplest circular dependency
        }

        bool checkCircularHelper(DO.Dependency item, int dependentID)  //helper inner fucntion
        {
            IEnumerable<DO.Dependency?> chain;
            bool res;

            //get all the dependencies where the requisite id of item was a dependent id
            chain = from cur in _dal.Dependency.ReadAll(d=>d.DependentID==item.RequisiteID) //finding where our current requisite id is a dependent id
                    select cur; //find all the 
            
            foreach (var d in chain)
            {
                if (d.RequisiteID == dependentID) //if the requisite in it is the dependent id then we have a circular depdency
                    return true;
                res = checkCircularHelper(d, dependentID);  //check the requisite ids for for the requisite id in this dependency
                if (res) return res; // if we returned true here, return true, it was circular
            }
            return false; //we have checked all possibilities, it won't create a circular dependency
        }

        return checkCircularHelper(item, item.DependentID); // start the recursive calls

    }

    /// <summary>
    /// check the post start production does not update cor values of task
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlIllegalOperationException"></exception>
    private void validateTaskProduction(BO.Task task)
    {
        DO.Task dTask;
        try
        {
            dTask = _dal.Task.Read(task.ID);
        }
        catch(DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        //if any of the non-textual items changed, throw an error

        if (
            dTask.Created!=task.Created ||
            dTask.Deadline!=task.Deadline ||
            dTask.ProjectedStart!=task.ProjectedStart ||
            dTask.ActualStart!=task.ActualStart ||
            dTask.ActualEnd!=task.ActualEnd 
            )
        {
            throw new BlIllegalOperationException("Cannot manipulate core data of task after production started");
        }

    }


    /// <summary>
    /// helper method to creat the DO.Task from the BO.Task
    /// </summary>
    /// <param name="task"></param>
    /// <returns>a DO.Task</returns>

    private DO.Task getDOTask(BO.Task task)
    {
        return new DO.Task
        {
            ID = task.ID,
            Nickname = task.Name,
            Description = task.Descripiton,
            Milestone = false,
            Created = task.Created,
            ProjectedStart = task.ProjectedStart,
            ActualStart = task.ActualStart,
            Deadline = task.Deadline,
            Duration = getDurationDays(task),
            ActualEnd = task.ActualEnd,
            Deliverable = task.Deliverable,
            Notes = task.Notes,
            AssignedEngineer = task.Engineer != null ? task.Engineer.ID : null,
            Difficulty = (Experience?)task.Complexity,
        };
    }



    /// <summary>
    /// helper method that gets the BO.Task from the DO.Taks
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private BO.Task getBOTask(DO.Task task)
    {
        return new BO.Task
        {
            ID = task.ID,
            Name = task.Nickname,
            Descripiton = task.Description,
            Created = task.Created,
            Status = getStatus(task),
            Dependencies = getDependencies(task.ID),
            Milestone = null,
            ProjectedStart = task.ProjectedStart,
            ProjectedEnd = getProjectedEnd(task),
            ActualStart = task.ActualStart,
            ActualEnd = task.ActualEnd,
            Deadline = task.Deadline,
            Deliverable = task.Deliverable,
            Notes = task.Notes,
            Engineer = getEngineer(task.AssignedEngineer),
            Complexity = (EngineerExperience?)task.Difficulty
        };

    }


}

