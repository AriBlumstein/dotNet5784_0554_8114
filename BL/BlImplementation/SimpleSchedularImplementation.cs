



namespace BlImplementation;

using BlApi;
using BO;
using DalApi;
using DO;

internal class SimpleSchedularImplementation : ISchedular
{
    private IDal _dal = DalApi.Factory.Get;

    private readonly Bl _bl;

    internal SimpleSchedularImplementation(Bl bl)=> _bl = bl;
    
    
    public void createSchedule(DateTime projectedStart)
    {

        //did we already entered production
        try
        {
            _dal.Config.GetProjectStart();
            throw new BlIllegalOperationException("Production already began");
        }
        catch(DALConfigDateNotSet){}

        //make sure our projectStart>=The internal clock

        if(projectedStart<_bl.Clock)
        {
            throw new BlIllegalOperationException("Cannot enter production with a date in the past");
        }

        //we will check that each task has a duration, if it doesn't than we cannot make the schedule, also make sure difficulties are well defined, otherwise we can't assign an engineer to it and production can't continue
        IEnumerable<DO.Task> allDTasks = _dal.Task.ReadAll()!;

        foreach (DO.Task task in allDTasks)
        {
            if (task.Duration == null)
            {
                throw new BlIllegalOperationException($"Cannot make a schedule as task \"{task.ID}\" does not have a duration");
            }
            if (task.Difficulty == null)
            {
                throw new BlIllegalOperationException($"Cannot make a schedule as task \"{task.ID}\" does not have complexity that is well defined");
            }
        }

        DateTime projectedEnd = projectedStart; // to be used later when updating config

        //helper method to get the best earliest possible start date
        DateTime earliestDate(BO.Task task)
        {
            DateTime innerProjectedStart = projectedStart;  // from our original projected start

            foreach (var dep in task.Dependencies)
            {
                BO.Task depTask;
                
                depTask = _bl.Task.Read(dep.ID);


                if (innerProjectedStart<depTask.ProjectedEnd)
                {
                    innerProjectedStart=depTask.ProjectedEnd.Value; //projected end is becoming later and later
                }
               
            }

            return innerProjectedStart;

            
        }




        //we have now passed this test, so now we need to topologically sort the dependencies to give each Task a projected startdate
        IEnumerable<int> ids = topologicalSort(_dal.Dependency.ReadAll()!);

        
        //we will now iterate through this list assigning the dates properly

        foreach (int id in ids)
        {
            BO.Task cur = _bl.Task.Read(id);

            //update the projected start date

            cur=_bl.Task.UpdateProjectedStartDate(id, earliestDate(cur));

            //update the projected end as we see fit
            if(projectedEnd<cur.ProjectedEnd)
            {
                projectedEnd=cur.ProjectedEnd.Value;
            }
        }


        //now let us officially enter production
        _dal.Config.SetProjectStart(projectedStart);
        _dal.Config.SetProjectEnd(projectedEnd);
   
    }

    /// <summary>
    /// private method that will topologically sort the tasks
    /// </summary>
    /// <param name="dependencies"></param>
    /// <returns>a sorted list of ids for our automatic schedular</returns>
    private IEnumerable<int> topologicalSort(IEnumerable<DO.Dependency> dependencies)
    {
      
        Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();

        //helper method to add the edges
        void addEdge(int requisiteId, int dependentId)
        {
            if (!adjacencyList.ContainsKey(requisiteId))
            {
                adjacencyList[requisiteId] = new List<int>();
            }
            adjacencyList[requisiteId].Add(dependentId);
        }

        foreach (DO.Dependency dep in dependencies)
        {
            addEdge(dep.RequisiteID, dep.DependentID);
        }



        //helper method to actually sort the edges
        void topologicalSortUtil(int taskId, HashSet<int> visited, Stack<int> stack)
        {
            visited.Add(taskId);

            if (adjacencyList.ContainsKey(taskId))
            {
                foreach (var dependentId in adjacencyList[taskId])
                {
                    if (!visited.Contains(dependentId))
                    {
                        topologicalSortUtil(dependentId, visited, stack);
                    }
                }
            }

            stack.Push(taskId);

        }


        var visited = new HashSet<int>();
        var stack = new Stack<int>();

        foreach (var taskId in _dal.Task.ReadAll().Select(i => i!.ID))
        {
            if (!visited.Contains(taskId))
            {
                topologicalSortUtil(taskId, visited, stack);
            }
        }

      

        return stack.ToList();  //the topological sorted IDs


    }
}
