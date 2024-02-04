
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task> ReadAll(Func<DO.Task, bool> filter = null)
    {
        IEnumerable<DO.Task?> dTasks = _dal.Task.ReadAll(filter);
        return dTasks.Select(t => new BO.Task
        {
            ID = t.ID,
            Name=t.Nickname,
            Descripiton = t.Description,
            Created = t.Created,
            Status = getStatus(t),
            Dependencies=getDependencies(t.ID),
            Milestone=null,
            ProjectedStart=t.ProjectedStart,
            ProjectedEnd=getProjectedEnd(t),
            ActualStart=t.ActualStart,
            ActualEnd=t.ActualEnd,
            Deadline=t.Deadline,
            Deliverable=t.Deliverable,
            Notes=t.Notes,
            Engineer=getEngineer(t.AssignedEngineer),
            Complexity=(EngineerExperience)t.Difficulty


        }) ;
    }


    public BO.Task Update(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void UpdateProjectedStartDate(int id, DateTime startDate)
    {
        throw new NotImplementedException();
    }

    private Status? getStatus(DO.Task task)
    {
        if (task.ProjectedStart == null) return Status.Unscheduled;
        else if (task.ProjectedStart >= DateTime.Now) return Status.Scheduled;
        else if (task.ActualEnd == null) return Status.OnTrack;
        else if (task.ActualEnd <= DateTime.Now) return Status.Completed;
        return null;
    }

    private List<TaskInList?> getDependencies(int taskID)
    {
        IEnumerable<int> taskIDs = _dal.Dependency.ReadAll(i=>i.DependentID==taskID).Select(i=>i.RequisiteID);


        return taskIDs.Select(i=>getTaskInList(i)).ToList();
    }

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

    private BO.TaskInList? getTaskInList(int taskID)
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
}
