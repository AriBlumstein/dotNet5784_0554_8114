﻿



namespace BlImplementation;
using BlApi;
using BO;
using DalApi;

internal class MilestoneImplementation : IMilestone
{


    private static DalApi.IDal _dal = DalApi.Factory.Get;

    Lazy<int> _numTasks = new Lazy<int>(() => _dal.Task.ReadAll().Count()); /// the milestone tasks will be the last tasks in the database, but we want them to be numbered from 0 when we deal with them

    public void CreateProjectSchedule()
    {
       
    }

    public Milestone Read(int id)
    {
        return null;
    }

    public Milestone Update(int id)
    {
        return null;
    }
}
