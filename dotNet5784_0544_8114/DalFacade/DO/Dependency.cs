
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
    DateTime? DeliveryDate = null,
    bool Active = true
)
{
    /// <summary>
    /// empty constuctor(
    /// </summary>
    public Dependency(): this(-1,-1,-2,"","", DateTime.Now) { }

    public override string ToString()
    {
        return $"""
            ID: {ID}
            DependentID: {DependentID}
            Requisite ID: {RequisiteID}
            Customer Email: {CustomerEmail}
            Address: {Address}
            Order Date: {OrderDate}
            Shipping Date: {(ShippingDate.HasValue ? ShippingDate : "null")}
            Delivery Date: {(DeliveryDate.HasValue ? DeliveryDate : "null")}
            """;
    }

}
