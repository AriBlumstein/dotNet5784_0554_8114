

namespace DO;

/// <summary>
/// data structure that represents a dependency
/// </summary>

public record Dependency
{ 



    /// <summary>
    /// parameter constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dependentID"></param>
    /// <param name="requisiteID"></param>
    /// <param name="customerEmail"></param>
    /// <param name="address"></param>
    /// <param name="orderDate"></param>
    /// <param name="shippingDate"></param>
    /// <param name="deliveryDate"></param>
    public Dependency( int id, int dependentID, int requisiteID, string customerEmail, string address, DateTime orderDate, DateTime? shippingDate, DateTime? deliveryDate)
    {
        ID=id;
        DependentID = dependentID;
        RequisiteID = requisiteID;
        CustomerEmail = customerEmail;
        Address = address;
        OrderDate = orderDate;
        ShippingDate = shippingDate;
        DeliveryDate = deliveryDate;
    }

    public Dependency() { }

    public int ID { get; init; }
    public int DependentID { get; set; } //what depends on me
    public int RequisiteID { get; set; } //what I depend upon
    public String CustomerEmail { get; set; }
    public String Address { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippingDate { get; set; } = null;
    public DateTime? DeliveryDate { get; set; } = null;
}
