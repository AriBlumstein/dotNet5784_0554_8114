
namespace DO;


/// <summary>
/// Dependency relation
/// </summary>
/// <param name="ID"></param>
/// <param name="DependentID"></param> // what is dependent
/// <param name="RequisiteID"></param> //what needs to be done
/// <param name="CustomerEmail"></param>
/// <param name="Address"></param>
/// <param name="OrderDate"></param>
/// <param name="ShippingDate"></param>
/// <param name="DeliveryDate"></param>
public record Dependency
(


    int ID,
    int DependentID, //what depends on me
    int RequisiteID, //what I depend upon
    String CustomerEmail,
    String Address,
    DateTime OrderDate,
    DateTime? ShippingDate = null,
    DateTime? DeliveryDate = null
)
{
    /// <summary>
    /// empty constuctor(
    /// </summary>
    public Dependency(): this(-1,-1,-1,"","", DateTime.Now) { }

}
