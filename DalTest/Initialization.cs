
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
            Experience e=randExpereince();

            //get a create random date
            DateTime create = DateTime.Now.AddDays(-(s_rand.Next(0, 100)));

         

            //get the duration
            int duration = s_rand.Next(1, 100);

            //create the task, the id can be -1, because we are here coming from the "business layer simulation" and can update later
            s_dal!.Task.Create(new Task(-1, name, descriptions[Array.IndexOf(names, name)], false, create, null, null, null, duration, null, null, null, null, e));
        }

       
    }

    /// <summary>
    /// initalize engineers
    /// </summary>
    private static void createEngineers() {

        const int MIN_ID = 200000000, MAX_ID = 400000000;
        
        String[] names = new string[] { "Ariel", "Eliyahu", "Benji", "Binyamin", "David" };
        foreach (var name in names)
        {
            int id;
            bool unique = false;
            DO.Engineer? cur;
            do {
                id = s_rand.Next(MIN_ID, MAX_ID);
                try
                {
                    //see if the the id does not already exist
                    cur = s_dal!.Engineer!.Read(id);
                }
                catch(Exception ex)
                {
                    unique= true;
                }
            
               }
            while (!unique);

            //get random experience
            Experience e = randExpereince();

            //get random salary
            double rate = (double)s_rand.Next(200, 10000);


            s_dal!.Engineer.Create(new Engineer(id, name, rate, name + "@company.com", e));
        }

      
    }

    /// <summary>
    /// initialize dependencies
    /// </summary>
    private static void createDependencies() {

        
        // for the sake of making 2 tasks be dependent on the same things, we will pick the 2 tasks with the latest projected start
        // as not to get stuck with picking one that cannot be a dependent on anything

        int latestTask=-1;
        //DateTime? latestTime=s_dal!.Config.GetProjectStart();

        
        IEnumerable<DO.Task> tasks = s_dal!.Task!.ReadAll().ToList()!;
        List<int> taskIDs = tasks.Select(tsk => tsk.ID).ToList();

        /*

        foreach(var cur in tasks)
        {
            if(cur.ProjectedStart>latestTime)
            {
                latestTask = cur.ID;
                latestTime = cur.ProjectedStart;
            }
        }

        //now we will pick our second task

        int secondLatestTask=-1;
        latestTime = s_dal!.Config.GetProjectStart();


        foreach (var cur in tasks)
        {
            if (cur.ProjectedStart > latestTime && cur.ID!=latestTask)
            {
                secondLatestTask = cur.ID;
                latestTime = cur.ProjectedStart;
            }
        }

        //now that we have both of these task, let us create dependencies, where both taks are dependent on the same things
        for(int i=0; i<3; ++i)
        {
            //get a requisite id that is not the same as either of the dependent ids
            int reqId;
            do
            {
                reqId = s_rand.Next(1, tasks.Count() + 1);
            } while (reqId == latestTask || reqId == secondLatestTask);


            //make the dependencies and check there is no time conflict and put them in the database
            Dependency _firstD = new Dependency(-1, latestTask, reqId);
            Dependency _secondD = new Dependency(-1, secondLatestTask,reqId);

            if(TimeConflict(_firstD)||TimeConflict(_secondD))
            {
                --i;
                continue;
            }

            s_dal!.Dependency.Create(_firstD);
            s_dal!.Dependency.Create(_secondD);
        } 

        */
       

        // create the remaining random dependencies 
        for (int i = 0; i < 40; i++)
        {
            //get random task numbers
            int rand1 = s_rand.Next(0, tasks.Count()), rand2 =s_rand.Next(0,tasks.Count());

            int dependentID = taskIDs[rand1], requisiteID = taskIDs[rand2];

            Dependency oldExisted = s_dal.Dependency.Read(d => d.DependentID == dependentID && d.RequisiteID == requisiteID);

            if(oldExisted != null)
            {
                i--;
                continue;
            }
            
           
       
            DO.Dependency newD = new Dependency(-1, dependentID, requisiteID);
           
            //if it creates a circular dependency or is a time conflict
            if (TimeConflict(newD)||CheckCircularDependency(newD)){
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

    public static void Do()
    {
        
       s_dal = Factory.Get;
        
        s_dal.Reset(); //reset the entire database
        
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
    static bool CheckCircularDependency(DO.Dependency item)
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
            chain = s_dal!.Dependency.ReadAll().ToList().FindAll(i => i.DependentID == item.RequisiteID && i.Active);
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
    static bool TimeConflict(Dependency cur)
    {
        DO.Task? dependent = s_dal!.Task.Read(cur.DependentID), _requisite = s_dal!.Task.Read(cur.RequisiteID);
        return dependent!.ProjectedStart < _requisite!.Deadline;
       
    }


}
