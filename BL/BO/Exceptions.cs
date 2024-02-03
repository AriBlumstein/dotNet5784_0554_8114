

namespace BO;


[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string? message, Exception innerException) : base(message, innerException) { }
}


[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

[Serializable]

public class BlIllegalPropertyException : Exception
{
    public BlIllegalPropertyException(string? message) : base(message) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message, Exception e) : base(message, e) { }
}

[Serializable]
public class BlIllegalOperationException : Exception
{
    public BlIllegalOperationException(string? message) : base(message) { }
}
