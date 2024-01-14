
namespace DalTest;
using DO;
using DalApi;
using System.Collections.Generic;

/// <summary>
/// class to initialize the databases for the purpose of the test
/// </summary>
public static class Initialization
{
    

    private static IDal? s_dal;
   

    private static readonly Random s_rand= new ();
    

    /// <summary>
    /// initiailize our project start and ends
    /// </summary>
    private static void initConfig()
    {
        s_dal!.Config.setProjectStart(DateTime.Now.AddDays(s_rand.Next(1, 60)));

        s_dal!.Config.setProjectEnd(s_dal!.Config.getProjectStart().AddMonths(s_rand.Next(18,37)));
    }
   


    /// <summary>
    /// get a random experience level
    /// </summary>
    /// <returns>Experience</returns>
    private static Experience randExpereince()
    {
        return (Experience)s_rand.Next(0, Enum.GetValues(typeof(Experience)).Length);
    }


    /// <summary>
    /// initialize random tasks
    /// </summary>

    private static void createTasks() {


        int numDays = (s_dal!.Config.getProjectEnd() - s_dal!.Config.getProjectStart()).Days;


        string[] names = new string[]
        {
            "Structural Analysis and Design",
            "Systems Integration Testing",
            "Finite Element Analysis (FEA)",
            "Prototyping and Validation",
            "Project Scheduling and Planning",
            "Environmental Impact Assessment",
            "Design Optimization",
            "Materials Selection and Testing",
            "Quality Assurance and Control",
            "Regulatory Compliance Review",
            "Cost Estimation and Budgeting",
            "Geotechnical Investigation",
            "Risk Assessment and Management",
            "3D Modeling and Rendering",
            "Failure Mode and Effect Analysis (FMEA)",
            "Field Inspection and Monitoring",
            "Energy Efficiency Analysis",
            "Simulation and Modeling",
            "Technical Documentation and Reporting",
            "Supplier and Vendor Coordination"
        };


        string[] descriptions = new string[]
        {
            "Analyzing and designing structures for various projects.",
            "Conducting testing to ensure seamless integration of systems.",
            "Performing Finite Element Analysis for structural simulations.",
            "Creating prototypes and validating their functionality.",
            "Planning and scheduling tasks for successful project completion.",
            "Assessing and mitigating environmental impacts of engineering projects.",
            "Optimizing designs for efficiency and performance.",
            "Selecting materials and conducting testing for suitability.",
            "Ensuring and controlling the quality of engineering processes.",
            "Reviewing and ensuring compliance with regulatory standards.",
            "Estimating project costs and creating budgets.",
            "Investigating soil and rock properties for construction projects.",
            "Assessing and managing risks associated with engineering projects.",
            "Creating detailed 3D models and realistic renderings.",
            "Analyzing potential failure modes and their effects.",
            "Inspecting and monitoring projects in the field.",
            "Analyzing and improving energy efficiency in designs.",
            "Conducting simulations and mathematical modeling.",
            "Preparing technical documentation and reports.",
            "Coordinating with suppliers and vendors for project components."
        };


        foreach (string name in names)
        {
            //get a random Experience
            Experience _e=randExpereince();

            //get a create random date
            DateTime _create = DateTime.Now.AddDays(-(s_rand.Next(0, 100)));

            //make random start and ends to this task
            DateTime _projectedStart;
            DateTime _deadline;


            do
            {                                                                     
                _projectedStart = s_dal!.Config.getProjectStart().AddDays(s_rand.Next(1, numDays));
                _deadline = s_dal!.Config.getProjectEnd().AddDays(-(s_rand.Next(1, numDays)));
                                                          
            }      //make sure the start is before the deadline
            while (_projectedStart > _deadline);
            // the logic here prevents projectedStarts before the projectStart and projectedEnds after the projectEnd


            //get the duration
            int _duration = (_deadline - _projectedStart).Days;

            //create the task, the id can be -1, because we are here coming from the "business layer simulation" and can update later
            s_dal!.Task.Create(new Task(-1, name, descriptions[Array.IndexOf(names, name)], false, _create, _projectedStart, null, _deadline, _duration, null, null, null, null, _e));
        }

       
    }

    /// <summary>
    /// initalize engineers
    /// </summary>
    private static void createEngineers() {

        const int MIN_ID = 200000000, MAX_ID = 400000000;

        String[] names = new string[] { "Ariel", "Eliyahu", "Benji", "Binyamin", "David" };
        foreach (var _name in names)
        {
            int _id;
            bool unique = false;
            DO.Engineer? cur;
            do {
                _id = s_rand.Next(MIN_ID, MAX_ID);
                try
                {
                    //see if the the id does not already exist
                    cur = s_dal!.Engineer!.Read(_id);
                }
                catch(Exception ex)
                {
                    unique= true;
                }
            
               }
            while (!unique);

            //get random experience
            Experience _e = randExpereince();

            //get random salary
            double _rate = (double)s_rand.Next(200, 10000);


            s_dal!.Engineer.Create(new Engineer(_id, _name, _rate, _name + "@company.com", _e));
        }

      
    }

    /// <summary>
    /// initialize dependencies
    /// </summary>
    private static void createDependencies() {

        // for the sake of making 2 tasks be dependent on the same things, we will pick the 2 tasks with the latest projected start
        // as not to get stuck with picking one that cannot be a dependent on anything

        int _latestTask=-1;
        DateTime? _latestTime=s_dal!.Config.getProjectStart();

        List<DO.Task> tasks = s_dal!.Task!.ReadAll();

        foreach(var cur in tasks)
        {
            if(cur.ProjectedStart>_latestTime)
            {
                _latestTask = cur.ID;
                _latestTime = cur.ProjectedStart;
            }
        }

        //now we will pick our second task

        int _secondLatestTask=-1;
        _latestTime = s_dal!.Config.getProjectStart();


        foreach (var cur in tasks)
        {
            if (cur.ProjectedStart > _latestTime && cur.ID!=_latestTask)
            {
                _secondLatestTask = cur.ID;
                _latestTime = cur.ProjectedStart;
            }
        }

        //now that we have both of these task, let us create dependencies, where both taks are dependent on the same things
        for(int i=0; i<3; ++i)
        {
            //get a requisite id that is not the same as either of the dependent ids
            int _reqId;
            do
            {
                _reqId = s_rand.Next(1, tasks.Count() + 1);
            } while (_reqId == _latestTask || _reqId == _secondLatestTask);


            //make the dependencies and check there is no time conflict and put them in the database
            Dependency _firstD = new Dependency(-1, _latestTask, _reqId);
            Dependency _secondD = new Dependency(-1, _secondLatestTask,_reqId);

            if(timeConflict(_firstD)||timeConflict(_secondD))
            {
                --i;
                continue;
            }

            s_dal!.Dependency.Create(_firstD);
            s_dal!.Dependency.Create(_secondD);
        } 
       

        // create the remaining random dependencies 
        for (int i = 0; i < 34; i++)
        {
            //get random task numbers
            int rand1 = s_rand.Next(0, tasks.Count()), rand2 =s_rand.Next(0,tasks.Count());

            int dependentID = tasks[rand1].ID, requisiteID = tasks[rand2].ID;          
           
       
            DO.Dependency newD = new Dependency(-1, dependentID, requisiteID);
           
            //if it creates a circular dependency or is a time conflict
            if (timeConflict(newD)||checkCircularDependency(newD)){
                i--; //must try to make a new dependency 
                continue;
            }
            s_dal!.Dependency.Create(newD);

        }


    }

    /// <summary>
    /// initialze initialization
    /// </summary>
    /// <param name="dalTask"></param>
    /// <param name="dalEngineer"></param>
    /// <param name="dalDependency"></param>
    /// <exception cref="NullReferenceException"></exception>

    public static void Do(IDal? dal)
    {
        s_dal = dal ?? throw new Exception("S_DAL cannot be null");
       

        initConfig();
        
        createEngineers();
     
        createTasks();
       
        createDependencies();
        

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="item">the new dependency to check if it creates a circular dependency</param>
    /// <param name="curList">the list we want to insert it to</param>
    /// <returns>true if circular dependency, false otherwise</returns>
    static bool checkCircularDependency(DO.Dependency item)
    {

        if (item.RequisiteID == item.DependentID)
        {
            return true;
        }

        bool checkCircularHelper(DO.Dependency item, int dependentID)
        {
            List<DO.Dependency> chain;
            bool res;
            //get all the dependencies where the requisite id of item was a dependent id
            chain = s_dal!.Dependency.ReadAll().FindAll(i => i.DependentID == item.RequisiteID && i.Active);
            foreach (var d in chain)
            {
                if (d.RequisiteID == dependentID)
                    return true;
                res = checkCircularHelper(d, dependentID);
                if (res) return res;
            }
            return false;
        }
        return checkCircularHelper(item, item.DependentID);

    }


    /// <summary>
    /// method checks if the depend task is scheduled before the requisite task, ie, the depdendent task is supposed to start before the requisite ends
    /// </summary>
    /// <param name="cur"></param>
    /// <returns>true if yes, flase if no</returns>
    static bool timeConflict(Dependency cur)
    {
        DO.Task? _dependent = s_dal!.Task.Read(cur.DependentID), _requisite = s_dal!.Task.Read(cur.RequisiteID);
        return _dependent!.ProjectedStart < _requisite!.Deadline;
       
    }


}
