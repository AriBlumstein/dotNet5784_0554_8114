namespace DO;




/// <summary>
/// exception for  a dal item not existing
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// excecption fro the item already existing
/// </summary>

[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class IllegalConfigAccessException: Exception
{
    public IllegalConfigAccessException(string? message) : base(message) { }
}


[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message): base(message) { }
}