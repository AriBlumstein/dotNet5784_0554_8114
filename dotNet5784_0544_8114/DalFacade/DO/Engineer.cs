
namespace DO;

/// <summary>
/// data object to represent an engineer
/// </summary>
/// <param name="ID"></param>
/// <param name="Name"></param>
/// <param name="Cost"></param>
/// <param name="Email"></param>
/// <param name="Exp"></param>
public record Engineer
(
    int ID,
    String Name,
    double Cost,
    String Email,
    Experience Exp,
    bool Active=true
)
{
   public Engineer() : this(0,"",0,"",Experience.Beginner) { }
    public override string ToString()
    {
        return $"""
              Name: {Name}
              Cost: {Cost}
              Email: {Email}
              Experience: {Exp}
              """;
    }
}
