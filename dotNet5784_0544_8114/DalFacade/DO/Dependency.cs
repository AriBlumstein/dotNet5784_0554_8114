

namespace DO;

public record Dependency
{ 


    private static int _cur = 0; //used for assigning id

    public Dependency(int? dependentID=null, int? requisiteID=null, string? customerEmail=null, string? address=null, DateTime? orderDate=null, DateTime? shippingDate=null, DateTime? deliveryDate=null)
    {
        ID = ++_cur;
        DependentID = dependentID;
        RequisiteID = requisiteID;
        CustomerEmail = customerEmail;
        Address = address;
        OrderDate = orderDate;
        ShippingDate = shippingDate;
        DeliveryDate = deliveryDate;
    }

    public Dependency() { }

    public int ID {  get; set; }
    public int? DependentID { get; set; } //what depends on me
    public int? RequisiteID { get; set; } //what I depend upon
    public String? CustomerEmail { get; set; }
    public String? Address { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
