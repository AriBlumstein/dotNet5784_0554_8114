

namespace BlTest;

using BlApi;
using BO;
using System.Reflection;

public static class Progarm
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
                    break;
                case "3":
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

            Console.WriteLine("Would you like to update the task in this engineer?");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                int taskId;
                Console.WriteLine("Enter the id of the task to add: ");
                while (!int.TryParse(Console.ReadLine(), out taskId)) Console.WriteLine("Please enter a number: ");
                task = new TaskInEngineer { ID = taskId };
                engineer.Task = task;
            }

            engineer.Name = name;
            engineer.Email = email;
            engineer.Cost = cost;
            engineer.Level = ee;

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

        } while (input != "0");
    }

}