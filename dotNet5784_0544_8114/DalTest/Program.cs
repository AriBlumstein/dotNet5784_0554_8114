using Dal;
using DalApi;
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
                """);
            input = Console.ReadLine();
            
            switch (input)
            {
                case "0":
                    return;
                case "1":
                    taskHandler();
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

    private static DO.Task createTask()
    {
        Console.WriteLine("Enter all of the relevant information seperated by a comma");
        Boolean _milestone;
        DateTime _date;
        string input = Console.ReadLine();
        List<string> elements = input.Split(',').ToList();
        if (Boolean.TryParse(elements[2], out _milestone) && DateTime.TryParse(elements[3], out _date))
            return new DO.Task(-1, elements[0], elements[1], _milestone, _date);
        throw new Exception("Error in input");

    }
}