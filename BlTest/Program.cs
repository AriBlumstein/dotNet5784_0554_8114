

namespace BlTest;

using BlApi;
using BO;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;

public static class Program
{
    static readonly IBl s_bl = BlApi.Factory.Get();

    public static void Main()
    {
        
        Console.WriteLine("Would you like to create initial data? ");
        string input = Console.ReadLine() ?? throw new FormatException("Invalid input");
        if (input.ToLower() == "y")
        {
            DalTest.Initialization.Do();
        }
        do
        {
            Console.WriteLine(
                """
                0) Quit
                1) Test Engineer
                2) Test Task
                3) Begin Production
                4) Reset Database/Start new project
                """
                );
            input = Console.ReadLine() ?? throw new FormatException("Invalid input");
            switch (input)
            {
                case "0":
                    return;
                case "1":
                    engineerHandler();
                    break;
                case "2":
                    taskHandler();
                    break;
                case "3":
                    productionHandler();
                    break;
                case "4":
                    Console.WriteLine("Are you sure you want to reset, (i.e. start a new project) (Y/N)?");
                    if (Console.ReadLine().ToLower()=="y")
                    {
                        s_bl.Reset();
                    }
                    break;
                default:
                    Console.WriteLine("not an option");
                    break;
            }

        } while (input.ToLower() != "0");
    }

    private static void engineerHandler()
    {
        void readAllHandler()
        {
            Console.WriteLine("Would you like to find engineers with a specific level? (y/n)");
            string input = Console.ReadLine() ?? throw new FormatException("Invalid input");
            IEnumerable<BO.Engineer> engineers;
            if (input.ToLower() == "y")
            {
                EngineerExperience ee;
                Console.WriteLine("Enter a level: ( Beginner, AdvancedBeginner, Intermediate, Advanced, Expert )");
                input = Console.ReadLine() ?? throw new FormatException("invalid input");
                while (!Enum.TryParse(input, out ee))
                {
                    Console.WriteLine("Invalid option: please choose one of the levels");
                    input = Console.ReadLine() ?? throw new FormatException("invalid input");
                }
                engineers = s_bl.Engineer.ReadAll(i => i.Exp == (DO.Experience)ee);

            }
            else
            {
                engineers = s_bl.Engineer.ReadAll();
            }

            foreach (var engineer in engineers)
            {
                Console.WriteLine(engineer);
                Console.WriteLine();
            }

        }

        void createHandler()
        {
            int id;
            string name, email;
            EngineerExperience ee;
            double cost;
            TaskInEngineer task = null;
            
            Console.WriteLine("Enter the engineer's id: ");
            while (!int.TryParse(Console.ReadLine(), out id)) Console.WriteLine("Please enter a number: ");

            Console.WriteLine("Enter the engineer's name: ");
            name = Console.ReadLine();
            
            Console.WriteLine("Enter the engineer's email: ");
            email = Console.ReadLine();

            Console.WriteLine("Enter the engineer's experience ( Beginner, AdvancedBeginner, Intermediate, Advanced, Expert )");
            while (!Enum.TryParse(Console.ReadLine(), out ee)) Console.WriteLine("Please enter a number: ");

            Console.WriteLine("Enter the engineer's salary/cost: ");
            while (!double.TryParse(Console.ReadLine(), out cost)) Console.WriteLine("Please enter a valid cost: ");

            Console.WriteLine("Would you like to add a task to this engineer?");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                int taskId;
                Console.WriteLine("Enter the id of the task to add: ");
                while (!int.TryParse(Console.ReadLine(), out taskId)) Console.WriteLine("Please enter a number: ");
                task = new TaskInEngineer {ID= taskId};
            }
            Console.WriteLine(s_bl.Engineer.Create(new BO.Engineer
            {
                ID = id,
                Name = name,
                Email = email,
                Cost = cost,
                Level = ee,
                Task = task,
            }));

        }

        void updateHandler(BO.Engineer engineer)
        {
            string name, email;
            EngineerExperience ee;
            double cost;
            TaskInEngineer task = null;

            Console.WriteLine("Enter the engineer's name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter the engineer's email: ");
            email = Console.ReadLine();

            Console.WriteLine("Enter the engineer's experience ( Beginner, AdvancedBeginner, Intermediate, Advanced, Expert )");
            while (!Enum.TryParse(Console.ReadLine(), out ee)) Console.WriteLine("Please enter a number: ");

            Console.WriteLine("Enter the engineer's salary/cost: ");
            while (!double.TryParse(Console.ReadLine(), out cost)) Console.WriteLine("Please enter a valid cost: ");

            Console.WriteLine("Would you like to update the task in this engineer? (not adding a task here will delete the old task if there was one for sake of test)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                int taskId;
                Console.WriteLine("Enter the id of the task to add: ");
                while (!int.TryParse(Console.ReadLine(), out taskId)) Console.WriteLine("Please enter a number: ");
                task = new TaskInEngineer { ID = taskId };
                
            }

            engineer.Name = name;
            engineer.Email = email;
            engineer.Cost = cost;
            engineer.Level = ee;
            engineer.Task = task;

            Console.WriteLine(s_bl.Engineer.Update(engineer));
        }

        string input="";
        do
        {
            try
            {
                Console.WriteLine(
                """
            0) Leave
            1) Read all engineers
            2) Read Engineer
            3) Add Engineer
            4) Delete Engineer
            5) Update Engineer
            6) Read Organized List of Engineers by Experience
            """
                );
                input = Console.ReadLine() ?? throw new FormatException("Invalid Input");
                int id;

                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        readAllHandler();
                        break;
                    case "2":
                        Console.WriteLine("Enter the engineer ID: ");
                        while (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Please enter a number: ");
                        }
                        Console.WriteLine(s_bl.Engineer.Read(id));
                        break;
                    case "3":
                        createHandler();
                        break;
                    case "4":
                        Console.WriteLine("Enter the engineer ID to delete: ");
                        while (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Please enter a number: ");
                        }
                        s_bl.Engineer.Delete(id);
                        break;
                    case "5":
                        Console.WriteLine("Enter the engineer ID to update: ");
                        while (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Please enter a number: ");
                        }
                        Engineer engineer = s_bl.Engineer.Read(id);
                        updateHandler(engineer);
                        break;
                    case "6":
                        var grouping = s_bl.Engineer.ReadGroupsOfExperience();
                        foreach(var group in grouping)
                        {
                            Console.WriteLine($"{group.Key}:");
                            foreach(var bEngineer in group)
                            {
                                Console.WriteLine(bEngineer);
                            }
                            Console.WriteLine();
                        }
                        break;
                    default:
                        Console.WriteLine("Not a valid option");
                        break;


                }
            } catch (BlAlreadyExistsException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
                Console.WriteLine($"Inner Exception:{e.InnerException!.GetType()}");

            }
            catch (BlIllegalOperationException e){
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (BlDoesNotExistException e) {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
                Console.WriteLine($"Inner Exception:{e.InnerException!.GetType()}");
            }
            catch (BlIllegalPropertyException e) {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (BlNullPropertyException e) {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        } while (input != "0");
    }

    private static void productionHandler()
    {
        string answer;
        Console.WriteLine("Are you sure you want to start production? Once production starts you will limited in what you can update in the database and the schedule will be set. The schedule will be automatically set.");
        answer = Console.ReadLine();
        if (answer.ToLower() == "y")
        {
            
            DateTime start; 
            Console.WriteLine("Enter the project start date: ");
            while (!DateTime.TryParse(Console.ReadLine(), out start)) Console.WriteLine("Enter a valid date");
            try
            {

            s_bl.Schedular.createSchedule(start);
            }
            catch (BlIllegalOperationException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (BlIllegalPropertyException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }

    private static void taskHandler()
    {

        void readAllHandler()
        {
            Console.WriteLine("In production the tasks will be returned in order by their projected start date");
            Console.WriteLine("Would you like to find tasks with a specific complexity? (y/n)?");
            string input = Console.ReadLine() ?? throw new FormatException("Invalid input");
            IEnumerable<BO.Task> tasks;
            if (input.ToLower() == "y")
            {
                EngineerExperience ee;
                Console.WriteLine("Enter a level: ( Beginner, AdvancedBeginner, Intermediate, Advanced, Expert )");
                input = Console.ReadLine() ?? throw new FormatException("invalid input");
                while (!Enum.TryParse(input, out ee))
                {
                    Console.WriteLine("Invalid option: please choose one of the levels");
                    input = Console.ReadLine() ?? throw new FormatException("invalid input");
                }
                tasks = s_bl.Task.ReadAll(i => i.Difficulty == (DO.Experience)ee);

            }
            else
            {
                tasks = s_bl.Task.ReadAll();
            }

            foreach (var task in tasks)
            {
                Console.WriteLine(task);
                Console.WriteLine();
            }
        }

        void createHandler()
        {
          

            string name, description;
            List<TaskInList> dTasks=new List<TaskInList>();
            int dTask;
            int duration;
            String deliverable, notes;
            EngineerInTask engineer=null;
            int engineerId;
            EngineerExperience ee;

            Console.WriteLine("Enter the task name: ");
            name = Console.ReadLine();

            Console.WriteLine("Enter the task description: ");
            description = Console.ReadLine();

            Console.WriteLine("Would you like to add dependencies (y/n): ");
            
            if(Console.ReadLine().ToLower()=="y")
            {
                do
                {
                    Console.WriteLine("enter the id of the requisite task: ");
                    while (!int.TryParse(Console.ReadLine(), out dTask)) { Console.WriteLine("Enter a valid number"); }

                    dTasks.Add(new TaskInList { ID = dTask });

                    Console.WriteLine("would you like to add another requisite task (y/n): ");


                } while (Console.ReadLine().ToLower() != "n");
            }

            Console.WriteLine("Enter a duration: ");
            
            while(!int.TryParse(Console.ReadLine(), out duration)) { Console.WriteLine("Enter an integer"); }

            Console.WriteLine("Enter deliverable:");
            deliverable= Console.ReadLine();

            Console.WriteLine("Enter notes:");

            notes = Console.ReadLine();

            Console.WriteLine("Enter the complexity:");

            while (!Enum.TryParse(Console.ReadLine(), out ee)) { Console.WriteLine("Enter a proper complexity:"); }


            Console.WriteLine("Would you like to add an engineer to this task(y/n)?");

            if(Console.ReadLine().ToLower()=="y")
            {
                Console.WriteLine("Enter the id of the engineer you want to add:"); 
                while(!int.TryParse(Console.ReadLine(), out engineerId)) { Console.WriteLine("Enter an integer"); }

                engineer=new EngineerInTask { ID = engineerId };
            }

            Console.WriteLine(s_bl.Task.Create(new Task {Notes=notes,Name=name, Descripiton=description, Created=DateTime.Now, Dependencies=dTasks, Duration=duration, Deliverable=deliverable, Engineer=engineer, Complexity=ee }));

            
        }

        void deleteHandler()
        {
            int id;
            Console.WriteLine("Enter the id of the task you want to delete:");
            while(!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Enter an integer");
            }

            s_bl.Task.Delete(id);
        }


        void updateHandler()
        {
            int id;
            string name, description;
            int dTask;
            int duration;
            String deliverable, notes;
            EngineerInTask engineer = null;
            int engineerId;
            EngineerExperience ee;

            BO.Task cur;

            Console.WriteLine("Enter the id of the task you wish to update:");
            while (!int.TryParse(Console.ReadLine(), out id)) { Console.WriteLine("Enter an integer"); }

            cur = s_bl.Task.Read(id);

            Console.WriteLine("Enter the task name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter the task description:");
            description = Console.ReadLine();

            Console.WriteLine("Would you like to add dependencies (y/n)?");

            if (Console.ReadLine().ToLower() == "y")
            {
                do
                {
                    Console.WriteLine("enter the id of the requisite task:");
                    while (!int.TryParse(Console.ReadLine(), out dTask)) { Console.WriteLine("Enter a valid number"); }

                    cur.Dependencies.Add(new TaskInList { ID = dTask });

                    Console.WriteLine("would you like to add another requisite task (y/n)?");


                } while (Console.ReadLine().ToLower() != "n");
            }

            Console.WriteLine("Enter a duration");

            while (!int.TryParse(Console.ReadLine(), out duration)) { Console.WriteLine("Enter an integer"); }

            Console.WriteLine("Enter deliverable:");
            deliverable = Console.ReadLine();

            Console.WriteLine("Enter notes:");

            notes = Console.ReadLine();

            Console.WriteLine("Enter the complexity:");

            while (!Enum.TryParse(Console.ReadLine(), out ee)) { Console.WriteLine("Enter a proper complexity"); }


            Console.WriteLine("Would you like to add an engineer to this task: (y/n)");

            if (Console.ReadLine().ToLower() == "y")
            {
                Console.WriteLine("Enter the id of the engineer you want to add:");
                while (!int.TryParse(Console.ReadLine(), out engineerId)) { Console.WriteLine("Enter an integer"); }

                engineer = new EngineerInTask { ID = engineerId };
            }


            cur.Name = name;
            cur.Descripiton = description;
            cur.Duration= duration;
            cur.Deliverable = deliverable;
            cur.Notes = notes;
            cur.Engineer = engineer;
            cur.Complexity = ee;

            Console.WriteLine(s_bl.Task.Update(cur));


        }


        string input="";
        do
        {
            try
            {
                Console.WriteLine(
                    """
                    0) Return
                    1) Read all tasks
                    2) Read a specific task
                    3) Create a task
                    4) Delete a task
                    5) Update a task
                    """);

                input = Console.ReadLine() ?? throw new FormatException("Illegal input");

                switch (input)
                {
                    case "0":
                        return;
                    case "1":
                        readAllHandler();
                        break;
                    case "2":
                        int id;
                        Console.WriteLine("Enter the task ID: ");
                        while (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Please enter a number: ");
                        }
                        Console.WriteLine(s_bl.Task.Read(id));
                        break;
                    case "3":
                        createHandler();
                        break;
                    case "4":
                        deleteHandler();
                        break;
                    case "5":
                        updateHandler();
                        break;

                }
            }
            catch (BlAlreadyExistsException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
                Console.WriteLine($"Inner Exception:{e.InnerException!.GetType()}");

            }
            catch (BlIllegalOperationException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (BlDoesNotExistException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
                Console.WriteLine($"Inner Exception:{e.InnerException!.GetType()}");
            }
            catch (BlIllegalPropertyException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch (BlNullPropertyException e)
            {
                Console.WriteLine($"Type: {e.GetType()}");
                Console.WriteLine($"Message:{e.Message}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }



        } while (input != "0");
    }

}