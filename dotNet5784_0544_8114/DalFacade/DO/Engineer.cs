
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
    Experience Exp
)
{
   public Engineer() : this(0,"",0,"",Experience.Beginner) { }
}
