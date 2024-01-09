
namespace DalTest;
using DO;
using DalApi;

/// <summary>
/// class to initialize the databases for the purpose of the test
/// </summary>
public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;

    private static readonly Random s_rand= new ();
    

    /// <summary>
    /// get a random date
    /// </summary>
    /// <returns>DateTime</returns>
    private static DateTime randomOldDay()
    {
        DateTime start = new DateTime(2007, 1, 1);
        int range = (new DateTime(2017,1,1) - start).Days;
        return start.AddDays(s_rand.Next(range));
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

            //get a random date
            DateTime _start=randomOldDay();

            //random duration
            int _duration = s_rand.Next(5, 100);

            //random date
            DateTime _end=_start.AddDays(_duration);

            //create the task, the id can be -1, because we are here coming from the "business layer simulation" and can update later
            s_dalTask!.Create(new Task(-1, name, descriptions[Array.IndexOf(names, name)], false, _start, null, null, _end, _duration, null, null, null, null, _e));
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
                    cur = s_dalEngineer!.Read(_id);
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


            s_dalEngineer!.Create(new Engineer(_id, _name, _rate, _name + "@company.com", _e));
        }
    }

    /// <summary>
    /// initialize dependencies
    /// </summary>
    private static void createDependencies() {
        List<Task> tasks = s_dalTask!.ReadAll();

        // Create the required initial dependencies
        s_dalDependency!.Create(new Dependency(-1, 2, 1, "a@gmail.com", "1600 Pennsylvania Ave.", randomOldDay(), null, null));
        s_dalDependency!.Create(new Dependency(-1, 2, 5, "b@gmail.com", "1234 random rd.", randomOldDay(), null, null));
        s_dalDependency!.Create(new Dependency(-1, 2, 6, "c@gmail.com", "123 sesame st", randomOldDay(), null, null));
        s_dalDependency!.Create(new Dependency(-1, 20, 1, "d@gmail.com", "100 franklin ave.", randomOldDay(), null, null));
        s_dalDependency!.Create(new Dependency(-1, 20, 5, "e@yahoo.com", "404 not found.", randomOldDay(), null, null));
        s_dalDependency!.Create(new Dependency(-1, 20, 6, "f@hotmail.com", "200 Ok. apt 1", randomOldDay(), null, null));
       
        
        // create the remaining random dependencies 
        for (int i = 0; i < 34; i++)
        {
            //get random task numbers
            int rand1 = s_rand.Next(0, tasks.Count()), rand2 =s_rand.Next(0,tasks.Count());

            int dependentID = tasks[rand1].ID, requisiteID = tasks[rand2].ID;          
           
            //get a random day
            DateTime dateTime = randomOldDay();
            try
            {
                s_dalDependency!.Create(new Dependency(-1, dependentID, requisiteID, "", "", dateTime, null, null));
            } catch (Exception ex) //catch if circular dependency was created.
            {
                i--; //must try to make a new dependency 
            }
        }
        

    }

    /// <summary>
    /// initialze initialization
    /// </summary>
    /// <param name="dalTask"></param>
    /// <param name="dalEngineer"></param>
    /// <param name="dalDependency"></param>
    /// <exception cref="NullReferenceException"></exception>

    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {
        String _except = "DAL cannot be null";
        s_dalTask = dalTask ?? throw new NullReferenceException(_except);
        s_dalEngineer= dalEngineer ?? throw new NullReferenceException(_except);
        s_dalDependency=dalDependency?? throw new NullReferenceException(_except);

        createEngineers();
        createTasks();
        createDependencies();

    }

}
