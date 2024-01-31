
namespace DalApi;

/// <summary>
/// interface to deal with the configuration
/// </summary>
public interface IConfig
{
    void SetProjectStart(DateTime start);
    void SetProjectEnd(DateTime end);
    DateTime GetProjectStart();    
    DateTime GetProjectEnd();

    void Reset();

}
