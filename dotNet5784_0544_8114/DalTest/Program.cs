using Dal;
using DalApi;
using DO;
using System.Linq.Expressions;

namespace DalTest;

internal class Program
{

    private static ITask? s_dalTask = new TaskImplementation();
    private static IEngineer? s_dalEngineer = new EngineerImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();
    static void Main(string[] args)
    {
        Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
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
                        //We expect an Nickname, Description, Milestone, Date Create YYYY-MM-DD
                        s_dalTask!.Create(createTask());
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the task");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dalTask!.Read(int.Parse(input)));
                        break;
                    case "d":
                        foreach (DO.Task t in s_dalTask!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();
                        }
                        break;
                    case "e":
                        Console.WriteLine("Enter the ID of the task you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dalTask!.Read(id)); // print the task
                        DO.Task updatedTask = createTask() with { ID = id };
                        s_dalTask.Update(updatedTask);
                        break;
                    case "f":
                        Console.WriteLine("Enter id of item you want to delete");
                        int del = int.Parse(Console.ReadLine());
                        s_dalTask!.Delete(del);
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
                        s_dalEngineer!.Create(createEngineer());
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the task");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dalEngineer!.Read(int.Parse(input)));
                        break;
                    case "d":
                        foreach (DO.Engineer t in s_dalEngineer!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();

                        }
                        break;
                    case "e":
                        Console.WriteLine("Enter the ID of the task you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dalEngineer!.Read(id)); // print the task
                        DO.Engineer updatedEngineer = createEngineer() with { ID = id };
                        s_dalEngineer.Update(updatedEngineer);
                        break;
                    case "f":
                        Console.WriteLine("Enter id of item you want to delete");
                        int del = int.Parse(Console.ReadLine());
                        s_dalEngineer!.Delete(del);
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
                        s_dalDependency!.Create(createDependency());
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the task");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dalDependency!.Read(int.Parse(input)));
                        break;
                    case "d":
                        foreach (DO.Dependency t in s_dalDependency!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();

                        }
                        break;
                    case "e":
                        Console.WriteLine("Enter the ID of the task you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dalDependency!.Read(id)); // print the task
                        DO.Dependency updatedDependency = createDependency() with { ID = id };
                        s_dalDependency.Update(updatedDependency);
                        break;
                    case "f":
                        Console.WriteLine("Enter id of item you want to delete");
                        int del = int.Parse(Console.ReadLine());
                        s_dalDependency!.Delete(del);
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
        Console.WriteLine("Enter all of the relevant information seperated by a comma - Nickname, Description, Mile stone, created");
        Boolean _milestone;
        DateTime _date;
        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();
        if (Boolean.TryParse(elements[2], out _milestone) && DateTime.TryParse(elements[3], out _date))
            return new DO.Task(-1, elements[0], elements[1], _milestone, _date);
        throw new Exception("Error in input");

    }

    private static DO.Engineer createEngineer()
    {
        Console.WriteLine("Enter all of the relevant information seperated by a comma - ID, name, cost, email, and level");
        int _ID;
        double _cost;
        Experience _level;

        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();

        if (int.TryParse(elements[0], out _ID) && double.TryParse(elements[2], out _cost) && Enum.TryParse(elements[4], out _level))
            return new DO.Engineer(_ID, elements[1], _cost, elements[3], _level);
        throw new Exception("Error in input");

    }

    private static DO.Dependency createDependency()
    {
        Console.WriteLine("Enter all of the relevant information seperated by a comma - Dependent ID, Requisite ID, customer email, shipping address, order date");
        int _dID, _rID;
        DateTime _date;

        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();

        if (int.TryParse(elements[0], out _dID) && int.TryParse(elements[1], out _rID) && DateTime.TryParse(elements[4], out _date ))
            return new DO.Dependency(-1, _dID, _rID, elements[2], elements[3], _date);
        throw new Exception("Error in input");

    }


    static void  reset()
    {
        s_dalDependency!.Reset();
        s_dalEngineer!.Reset();
        s_dalTask!.Reset();
    }
}