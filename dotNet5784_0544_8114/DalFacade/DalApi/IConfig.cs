
namespace DalApi;

/// <summary>
/// interface to deal with the configuration
/// </summary>
public interface IConfig
{
    void setProjectStart(DateTime start);
    void setProjectEnd(DateTime end);
    DateTime getProjectStart();    
    DateTime getProjectEnd();

    void Reset();

}
