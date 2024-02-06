// See https://aka.ms/new-console-template for more information

BO.Task task = new BO.Task { ID = 2, Name="Eliyahu", Descripiton="Eliyahu", Created=DateTime.Now };

List<BO.TaskInList> deps= new List<BO.TaskInList>();

deps.Add(new BO.TaskInList { Name = "Ariel", Description = "Ariel", ID = 2 });

deps.Add(new BO.TaskInList { Name = "kkk" });

task.Dependencies = deps;


BO.EngineerInTask en=new BO.EngineerInTask { ID = 2, Name = "grape" };

task.Engineer = en;

Console.WriteLine(task);