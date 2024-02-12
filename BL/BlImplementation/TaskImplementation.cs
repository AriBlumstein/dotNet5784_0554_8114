
namespace BlImplementation;


using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
        catch (DALConfigDateNotSet)
        {

        }

        //we can now add a task
        validateTask(task); //check the the task is valid

        int taskId;

        //try adding it to the database
        try
        {
            taskId=_dal.Task.Create(getDOTask(task));
        }
        catch (DalAlreadyExistsException e)
        {
            throw new BlAlreadyExistsException(e.Message, e);
        }

   

        //add all the dependencies, 
        IEnumerable<DO.Dependency> dependencies = task.Dependencies.Select(t => new DO.Dependency { ID=-1, DependentID = taskId, RequisiteID = t.ID }); //taskID is the task that was just created

        //verify the dependencies
        verifyDependencies(dependencies); //the only important check here is to see that the Requisite tasks exist

        foreach (DO.Dependency dep in dependencies)
        {
            _dal.Dependency.Create(dep);
        }

        return Read(taskId); //will have the proper updated info of the task created
    }
    public void Delete(int id)
    {
        //see if we are in production
        try
        {
            _dal.Config.GetProjectStart();

            throw new BlIllegalOperationException("cannot delete task during production");

        }
        catch (DALConfigDateNotSet) { }

        //check that it isn't a requisite for other tasks
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d => d.RequisiteID == id);

        if (dependencies.Count() != 0)
        {
            throw new BlIllegalOperationException($"Cannot delete task {id} as {dependencies.Count()} tasks are dependent on it ");
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

        //we now have to delete all of the dependencies that we were dependent on
        IEnumerable<int> depIds=_dal.Dependency.ReadAll(d=>d.DependentID== id).Select(d=>d.ID);

        foreach(var depId in depIds)
        {
            _dal.Dependency.Delete(depId);
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

    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return _dal.Task.ReadAll(filter).Select(t => new TaskInList
        {
            ID = t.ID,
            Description = t.Description,
            Name = t.Nickname,
            Status = getStatus(t)
        }); 
    
 
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
        catch (DALConfigDateNotSet) { }


        //the new dependencies that need to be verified
        IEnumerable<DO.Dependency> dependencies = task.Dependencies.Select(d => new DO.Dependency { DependentID = task.ID, RequisiteID = d.ID })
                                                                                .Where
                                                                                    (d => (_dal.Dependency.Read(cur => cur.DependentID == d.DependentID
                                                                                     && cur.RequisiteID == d.RequisiteID)) == null); //the dependencies that did not yet exist

        //if we are not in production we can update the dates and add dependencies
        if (!production)
        {
            validateTask(task);
            verifyDependencies(dependencies);
        }
        else  //we cannot update the dates or dependencies
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

        //we will now add the added dependencies if we are not in production
        if (!production)
        {
            //add those dependencies
            foreach (DO.Dependency dep in dependencies)
                _dal.Dependency.Create(dep);
        }


 

        return Read(task.ID); //the newly proper updated task

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
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d => d.DependentID == id);

        IEnumerable<twoDate> dependenciesTimes;

        try
        {
            //get the requisite tasks dates
            dependenciesTimes = from dep in dependencies
                                let rTask = this.Read(dep.RequisiteID) //read the task that we are dependent on
                                select new twoDate { Start = rTask.ProjectedStart, End = rTask.ProjectedEnd }; //get the dates we need to check
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
                throw new BlIllegalOperationException($"Cannot update task {id}'s start date as its dependencies have not been schduled yet");
            }
        }


        //now we can do the update
        BO.Task bTask = getBOTask(dTask);
        bTask.ProjectedStart = startDate;


        return this.Update(bTask); // does the production checks, and returns the the updated task
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
        else if (task.ActualStart < DateTime.Now) return Status.OnTrack;
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
        IEnumerable<TaskInList> dependencies = _dal.Dependency.ReadAll(i => i.DependentID == taskID).Select(i => i.RequisiteID).Select(id => getTaskInList(id));

        return dependencies.ToList();


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
                Name = task.Nickname,
                Status = getStatus(task),
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
        if (projectedStartNull && actualStartDateNull) return null;
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
    /// validate the properties of a task
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BlIllegalPropertyException"></exception>
    /// <exception cref="BlNullPropertyException"></exception>

    private void validateTask(BO.Task task)
    {
       
        if (task.Name == null)
        {
            throw new BlNullPropertyException("Must not be a null name");
        }
        if (task.Name.Length == 0)
        {
            throw new BlIllegalPropertyException("Name must be none empty");
        }
        if(task.Duration.HasValue)
        {
            if (task.Duration <= 0) 
            throw new BlIllegalPropertyException("Duration for a task cannot be less than or equal to 0");
        }

        //check that the engineer exists and that he has the proper skill level

        if (task.Engineer != null)
        {
            try
            {
                DO.Engineer dEngineer = _dal.Engineer.Read(task.Engineer.ID)!;

                //see that we have a complexity
                if (task.Complexity == null)
                {
                    throw new BlIllegalOperationException($"Assign a complexity to task {task.ID} before assigning an engineer");
                }

                //the task is too complex for the engineer
                if (task.Complexity > (EngineerExperience)dEngineer!.Exp)
                {
                    throw new BlIllegalOperationException($"the task is too complex for engineer {dEngineer.ID}");
                }

                DO.Task? anotherTask = _dal.Task.Read(t => t.AssignedEngineer == dEngineer.ID);

                if(anotherTask !=null && anotherTask.ID!= task.ID)
                {
                    throw new BlIllegalPropertyException($"Engineer {dEngineer.ID} is already assigned to task {anotherTask!.ID}");
                }

            }
            catch (DalDoesNotExistException ex) //the engineer didn't exist
            {
                throw new BlDoesNotExistException(ex.Message, ex);
            }
        }


    }


    /// <summary>
    /// method to verify the dependencies to be added
    /// </summary>
    /// <param name="dependencies"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlIllegalPropertyException"></exception>

    private void verifyDependencies(IEnumerable<DO.Dependency> dependencies)
    {
        //we will now check for a circular dependency and that the tasks we want to make as a dependency exist
        foreach (DO.Dependency dep in dependencies)
        {
            try
            {
                _dal.Task.Read(dep.RequisiteID);
            }
            catch (DalDoesNotExistException e)
            {
                throw new BlDoesNotExistException(e.Message, e); //the task we tried to to make as a dependency did not exist
            }

            if (checkCircularDependency(dep))  //the dependency will cause a circular dependency
            {
                throw new BlIllegalPropertyException($"Cannot create a circular dependency, {dep.RequisiteID} is already dependent on {dep.DependentID} ");
            }
        }
    }


    /// <summary>
    /// method checks if we insert a dependency will we have a circular dependency because of it
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true if there would be a circular dependency</returns>
    private bool checkCircularDependency(DO.Dependency item)
    {

        if (item.RequisiteID == item.DependentID)
        {
            return true;            // simplest circular dependency
        }

        bool checkCircularHelper(DO.Dependency item, int dependentID)  //helper inner function
        {
            IEnumerable<DO.Dependency?> chain;
            bool res;

            //get all the dependencies where the requisite id of item was a dependent id
            chain = from cur in _dal.Dependency.ReadAll(d => d.DependentID == item.RequisiteID) //finding where our current requisite id is a dependent id
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
        BO.Task oTask;
        try
        {
            oTask = Read(task.ID);
        }
        catch (DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e.Message, e);
        }

        //if any of the non-textual items changed, throw an error

        if (
            oTask.Created != task.Created ||
            oTask.Deadline != task.Deadline ||
            oTask.ProjectedStart != task.ProjectedStart ||
            oTask.ActualStart != task.ActualStart ||
            oTask.ActualEnd != task.ActualEnd ||
            oTask.Duration != task.Duration ||
            oTask.Complexity != task.Complexity 
            
       
            )
        {
            throw new BlIllegalOperationException("Cannot manipulate core data of task after production started");
        }

        //check that all the dependencies exist already
        if(task.Dependencies!=null)
        {
            foreach(var dep in task.Dependencies)
            {
                if (_dal.Dependency.Read(d=> d.RequisiteID==dep.ID && d.DependentID==task.ID)==null) // this dependency did not exist yet, we don't want to add another dependency
                {
                    throw new BlIllegalOperationException("Cannot create new dependencies during production");
                }
            }
        }

    }


    /// <summary>
    /// helper method to create the DO.Task from the BO.Task
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
            Duration = task.Duration,
            ActualEnd = task.ActualEnd,
            Deliverable = task.Deliverable,
            Notes = task.Notes,
            AssignedEngineer = task.Engineer != null ? task.Engineer.ID : null,
            Difficulty = (Experience?)task.Complexity,

            
           
        };
    }



    /// <summary>
    /// helper method that gets the BO.Task from the DO.Task
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
            Complexity = (EngineerExperience?)task.Difficulty,
            Duration= task.Duration
           
        };

    }


}

