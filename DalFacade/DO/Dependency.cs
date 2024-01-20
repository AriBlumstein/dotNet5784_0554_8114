
namespace DO;


/// <summary>
/// Dependency relation
/// </summary>
/// <param name="ID"></param>
/// <param name="DependentID"></param> // what is dependent
/// <param name="RequisiteID"></param> //what needs to be done
public record Dependency
(


    int ID,
    int DependentID, //what depends on me
    int RequisiteID, //what I depend upon
    bool Active = true
)
{
    /// <summary>
    /// empty constuctor(
    /// </summary>
    public Dependency(): this(-1,-1,-2) { }

    public override string ToString()
    {
        return $"""
            ID: {ID}
            DependentID: {DependentID}
            Requisite ID: {RequisiteID}
            """;
    }

}
