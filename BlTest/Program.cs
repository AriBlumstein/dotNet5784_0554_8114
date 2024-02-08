// See https://aka.ms/new-console-template for more information

using BlApi;
using DalApi;

DalTest.Initialization.Do();

IBl _bl = BlApi.Factory.Get();

IDal _dal = DalApi.Factory.Get;




_bl.Schedular.createSchedule(DateTime.Now.AddDays(100));


foreach (var task in _dal.Task.ReadAll())
{
    Console.WriteLine(task);
    Console.WriteLine();
}
