

namespace BlApi;


public class Factory
{
    public static IBl Get() => BlImplementation.Bl.Instance;
   
}
