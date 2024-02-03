

using BlApi;
using BO;

namespace BlImplementation;

public class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void CreateProjectSchedule()
    {
        throw new NotImplementedException();
    }

    public Milestone Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone Update(int id)
    {
        throw new NotImplementedException();
    }
}
