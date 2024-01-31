

namespace DalTest;

using DalApi;
using DO;


internal class Program
{

    static private readonly IDal s_dal = Factory.Get;

    static void Main(string[] args)
    {


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
                5. Initialize Data
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
                    s_dal.Reset();
                    break;
                case "5":
                    Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                    if (ans == "Y") //stage 3
                        try
                        {
                            Initialization.Do();
                        }
                        catch (DalXMLFileLoadCreateException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

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

                        id = s_dal!.Task.Create(createTask());
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
                        foreach (Task t in s_dal!.Task!.ReadAll())
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
                        Task updatedTask = createTask() with { ID = id };
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
            catch (DalXMLFileLoadCreateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DalDoesNotExistException dNex)
            {
                Console.WriteLine(dNex.Message);
            }
            catch (Exception ex) //catches any type of exception 
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
                        id = s_dal.Engineer!.Create(createEngineer());
                        Console.WriteLine($"the ID number for the Engineer is {id}");
                        break;
                    case "c":
                        Console.WriteLine("Enter the ID of the Engineer");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dal!.Engineer!.Read(int.Parse(input)));
                        Console.WriteLine();
                        break;
                    case "d":
                        foreach (Engineer t in s_dal!.Engineer!.ReadAll())
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
                        Engineer updatedEngineer = createEngineer(true) with { ID = id };
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
            catch (DalXMLFileLoadCreateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DalDoesNotExistException dNex)
            {
                Console.WriteLine(dNex.Message);
            }
            catch (DalAlreadyExistsException dAex)
            {
                Console.WriteLine(dAex.Message);
            }
            catch (Exception ex)// catches any type of exception
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
                        Console.WriteLine("Enter the ID of the dependency");
                        input = Console.ReadLine();
                        Console.WriteLine(s_dal!.Dependency!.Read(int.Parse(input)));
                        Console.WriteLine();
                        break;
                    case "d":
                        foreach (Dependency t in s_dal!.Dependency!.ReadAll())
                        {
                            Console.WriteLine(t);
                            Console.WriteLine();

                        }
                        break;
                    case "e":
                        Console.WriteLine();
                        Console.WriteLine("Enter the ID of the dependency you want to update");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal!.Dependency!.Read(id)); // print the task
                        Console.WriteLine();
                        Dependency updatedDependency = createDependency() with { ID = id };
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
            catch (DalXMLFileLoadCreateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DalDoesNotExistException dNex)
            {
                Console.WriteLine(dNex.Message);
            }
            catch (Exception ex) //catches any type of exception
            {
                Console.WriteLine(ex.Message);
            }
        } while (input != "a");

    }

    private static Task createTask()
    {
        Console.WriteLine("Enter all of the relevant information:");
  
                    
        string nickName;
        string description;
        bool milestone;
        DateTime created;
        DateTime projectedStart;
        DateTime actualStart;
        DateTime deadline;
        int duration;
        DateTime actualEnd;
        string deliverable;
        string notes;
        int assignedEngineer;
        Experience level;
        bool error = false;

        Console.WriteLine("Enter task nickname");
        nickName = Console.ReadLine();

        Console.WriteLine("Enter description");
        description = Console.ReadLine();

        do
        {
            Console.WriteLine("Enter milestone");
            error = !bool.TryParse(Console.ReadLine(), out milestone);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter date create");
            error = !DateTime.TryParse(Console.ReadLine(), out created);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter projected start");
            error = !DateTime.TryParse(Console.ReadLine(), out projectedStart);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter date actual start");
            error = !DateTime.TryParse(Console.ReadLine(), out actualStart);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter deadline");
            error = !DateTime.TryParse(Console.ReadLine(), out deadline);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);


        do
        {
            Console.WriteLine("Enter duration");
            error = !int.TryParse(Console.ReadLine(), out duration);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter actual end");
            error = !DateTime.TryParse(Console.ReadLine(), out actualEnd);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        Console.WriteLine("enter deliverable");
        deliverable=Console.ReadLine();

        Console.WriteLine("enter notes");
        notes = Console.ReadLine();

        do
        {
            Console.WriteLine("Enter assigned engineer");
            error = !int.TryParse(Console.ReadLine(), out assignedEngineer);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter difficulty");
            error = !Enum.TryParse(Console.ReadLine(), out level);
            if (error)
            {
                Console.WriteLine("Error, try again");
            }
        } while (error);

        return new Task(-1, nickName, description, milestone, created, projectedStart, actualStart, deadline, duration, actualEnd, deliverable, notes, assignedEngineer, level);












    }

    private static Engineer createEngineer(bool update=false)
    {
        Console.WriteLine("Enter all of the relevant information");
        int ID=0;
        string name;
        double cost;
        string email;
        Experience level;
        bool error = false;

        if (!update)
        {
            do
            {
                Console.WriteLine("Enter Engineer id");
                error=!int.TryParse(Console.ReadLine(), out ID);

                if(error)
                {
                    Console.WriteLine("bad input, try again");
                }

            } while (error);

        }

        Console.WriteLine("Enter Engineer name");
        name= Console.ReadLine();


        do
        {
            Console.WriteLine("Enter Engineer salary");
            error = !Double.TryParse(Console.ReadLine(), out cost);

            if (error)
            {
                Console.WriteLine("bad input, try again");
            }

        } while (error);

        Console.WriteLine("Enter Engineer email");
        email= Console.ReadLine();

        do
        {
            Console.WriteLine("Enter Engineer skill level");
            error = !Enum.TryParse(Console.ReadLine(), out level);

            if (error)
            {
                Console.WriteLine("bad input, try again");
            }

        } while (error);


        if (!update)
        {
            return new Engineer(ID, name, cost, email, level);
        }
        else
        {
            return new Engineer(-1, name, cost, email, level);
        }



    }

    private static Dependency createDependency()
    {
        Console.WriteLine("Enter all of the relevant information");
        int dID, rID;
   
        bool error = false;

        do
        {
            Console.WriteLine("Enter dependent id");

            error = !int.TryParse(Console.ReadLine(), out dID);
            if (error)
            {
                Console.WriteLine("bad input, try again");
            }
        } while (error);

        do
        {
            Console.WriteLine("Enter requisite id");

            error = !int.TryParse(Console.ReadLine(), out rID);
            if (error)
            {
                Console.WriteLine("bad input, try again");
            }
        } while (error);


        return new Dependency(-1,dID,rID);
    }
};