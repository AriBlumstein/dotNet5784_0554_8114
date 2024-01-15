using Dal;
using DalApi;
using DO;
using System.Linq.Expressions;

namespace DalTest;

internal class Program
{

    static private readonly IDal? s_dal = new DalList();

    static void Main(string[] args)
    {
        Initialization.Do(s_dal);
        string input;
        do
        {
            Console.WriteLine(
                """
                0. Exit the main menu
                1. Test out Task
                2. Test out Engineer
                3. Test out Dependency
                4. Reset All
                """);
            input = Console.ReadLine();
            
            switch (input)
            {
                case "0":
                    return;
                case "1":
                    taskHandler();
                    break;
                case "2":
                    engineerHandler();
                    break;
                case "3":
                    dependencyHandler();
                    break;
                case "4":
                    reset();
                    break;
                default:
                    break;
            }
        } while (input != "0");
        
    }

    private static string printOptions(string noun)
    {
        return $"""
            a) Go back
            b) Add  {noun} to the entity list - Create().
            c) Display {noun} using an {noun}’s identifier -Read().
            d) Display the {noun} list -ReadAll().
            e) Update {noun} - Update().
            f) Delete  {noun} from the {noun} list – Delete().
            """;
    }
    private static void taskHandler()
    {
        int id;
        string input;
        Console.WriteLine();

        do
        {
            Console.WriteLine(printOptions("task"));
            input = Console.ReadLine();
            try
            {
                switch (input)
                {
                    case "a":
                        return;
                    case "b":
                        
                        id=s_dal!.Task.Create(createTask());
                        Console.WriteLine($"the id of the task created it {id}");
                        Console.WriteLine();
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the task");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dal!.Task!.Read(int.Parse(input)));
                        Console.WriteLine();
                        break;
                    case "d":
                        foreach (DO.Task t in s_dal!.Task!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();
                        }
                        break;
                    case "e":
                        Console.WriteLine("Enter the ID of the task you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal!.Task.Read(id)); // print the task
                        Console.WriteLine();
                        DO.Task updatedTask = createTask() with { ID = id };
                        s_dal!.Task.Update(updatedTask);
                        break;
                    case "f":
                        Console.WriteLine("Enter id of item you want to delete");
                        int del = int.Parse(Console.ReadLine());
                        s_dal!.Task.Delete(del);
                        break;
                    default:
                        Console.WriteLine("Not one of the options");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (input != "a");
        
       


        
    }

    private static void engineerHandler()
    {
        int id;
        string input;

        do
        {
            Console.WriteLine(printOptions("engineer"));
            input = Console.ReadLine();
            Console.WriteLine();
            try
            {
                switch (input)
                {
                    case "a":
                        return;
                    case "b":
                        //We expect an Nickname, Description, Milestone, Date Create YYYY-MM-DD
                        id=s_dal.Engineer!.Create(createEngineer());
                        Console.WriteLine($"the ID number for the Engineer is {id}");
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the Engineer");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dal!.Engineer!.Read(int.Parse(input)));
                        Console.WriteLine();
                        break;
                    case "d":
                        foreach (DO.Engineer t in s_dal!.Engineer!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();

                        }
                        break;
                    case "e":
                        Console.WriteLine("Enter the ID of the Engineer you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal!.Engineer!.Read(id)); // print the task
                        Console.WriteLine();
                        DO.Engineer updatedEngineer = createEngineer() with { ID = id };
                        s_dal.Engineer.Update(updatedEngineer);
                        break;
                    case "f":
                        Console.WriteLine("Enter id of item you want to delete");
                        int del = int.Parse(Console.ReadLine());
                        s_dal!.Engineer!.Delete(del);
                        break;
                    default:
                        Console.WriteLine("Not one of the options");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (input != "a");





    }


    private static void dependencyHandler()
    {
        int id;
        string input;

        do
        {
            Console.WriteLine(printOptions("dependency"));
            input = Console.ReadLine();
            Console.WriteLine();
            try
            {
                switch (input)
                {
                    case "a":
                        return;
                    case "b":
                        //We expect an Nickname, Description, Milestone, Date Create YYYY-MM-DD
                        id = s_dal.Dependency!.Create(createDependency());
                        Console.WriteLine($"the id number of the dependency created is {id}");
                        Console.WriteLine();
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the task");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dal!.Dependency!.Read(int.Parse(input)));
                        Console.WriteLine();
                        break;
                    case "d":
                        foreach (DO.Dependency t in s_dal!.Dependency!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();

                        }
                        break;
                    case "e":
                        Console.WriteLine();
                        Console.WriteLine("Enter the ID of the task you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal!.Dependency!.Read(id)); // print the task
                        Console.WriteLine();
                        DO.Dependency updatedDependency = createDependency() with { ID = id };
                        s_dal.Dependency.Update(updatedDependency);
                        break;
                    case "f":
                        Console.WriteLine("Enter id of item you want to delete");
                        int del = int.Parse(Console.ReadLine());
                        s_dal!.Dependency!.Delete(del);
                        break;
                    default:
                        Console.WriteLine("Not one of the options");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (input != "a");

    }

    private static DO.Task createTask()
    {
        Console.WriteLine("""
                          Enter all of the relevant information seperated by a comma and no space: 
                          Nickname,Description,Milestone,created,projectedStart,actualStart,deadline,duration,actualEnd,deliverable,notes,assignedEngineer,difficulty
                          if no engineer,write -1
                          """);
        Boolean _milestone;
        DateTime _created, _projectedStart,_actualStart,_deadline,_actualEnd;
        int _duration;
        int _assignedEngineer;
        DO.Experience _e;
        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();
        if (Boolean.TryParse(elements[2], out _milestone)
            && DateTime.TryParse(elements[3], out _created)
            && DateTime.TryParse(elements[4], out _projectedStart)
            && DateTime.TryParse(elements[5], out _actualStart)
            && DateTime.TryParse(elements[6], out _deadline)
            && int.TryParse(elements[7], out _duration)
            && DateTime.TryParse(elements[8], out _actualEnd)
            && int.TryParse(elements[11], out _assignedEngineer)
            && Enum.TryParse(elements[12], out _e)
            )
        {
            int? _trueAssigned;
            if (_assignedEngineer == -1)
                _trueAssigned = null;
            else
                _trueAssigned= _assignedEngineer;

            return new DO.Task(-1, elements[0], elements[1], _milestone, _created, _projectedStart, _actualStart, _deadline, _duration, _actualEnd, elements[9], elements[10], _trueAssigned, _e);
        }
        throw new Exception("Error in input");

    }

    private static DO.Engineer createEngineer()
    {
        Console.WriteLine("Enter all of the relevant information seperated by a comma -name,cost,email,level");
        int _ID;
        double _cost;
        Experience _level;

        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();

        if (double.TryParse(elements[1], out _cost) && Enum.TryParse(elements[3], out _level))
            return new DO.Engineer(-1, elements[0], _cost, elements[2], _level);
        throw new Exception("Error in input");

    }

    private static DO.Dependency createDependency()
    {
        Console.WriteLine("Enter all of the relevant information seperated by a comma and no space- Dependent ID,Requisite ID");
        int _dID, _rID;
  
        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();

        if (int.TryParse(elements[0], out _dID) && int.TryParse(elements[1], out _rID))
            return new DO.Dependency(-1, _dID, _rID);
        throw new Exception("Error in input");

    }

    /// <summary>
    /// reset the databases
    /// </summary>
    static void reset()
    {
        s_dal!.Reset();
    }
}