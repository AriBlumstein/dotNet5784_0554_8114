﻿

using DO;

namespace DalApi;

public interface IDal 
{
   
    ITask Task { get; }
  
    IEngineer Engineer { get; }
    IDependency Dependency { get; }

    IConfig Config { get; }



}
