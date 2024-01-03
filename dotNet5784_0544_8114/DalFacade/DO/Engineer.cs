
namespace DO;

public record Engineer
{
    public Engineer(int id=0, string name="", double cost=0, String? email=null, Experience exp=Experience.Beginner)
    {
        this.ID = id;
        this.Name = name;
        this.Cost = Cost;  
        this.Email = email;
        this.Exp = exp;

    }

    public Engineer() { }

    public int ID { get; set; }
    public String Name { get; set; }
    public double Cost { get; set; }
    public String? Email { get; set; }
    public Experience Exp { get; set; }
}
