﻿

using DalApi;

namespace DalXml
{
    sealed public class DalXML : IDal
    {
        public ITask Task => throw new NotImplementedException();

        public IEngineer Engineer => new EngineerImplementation();

        public IDependency Dependency => throw new NotImplementedException();

        public IConfig Config => throw new NotImplementedException();

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
