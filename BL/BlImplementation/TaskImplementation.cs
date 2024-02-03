
namespace BlImplementation;
using BlApi;
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
        throw new NotImplementedException();
    }

    public BO.Task Update(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void UpdateProjectedStartDate(int id, DateTime startDate)
    {
        throw new NotImplementedException();
    }
}
