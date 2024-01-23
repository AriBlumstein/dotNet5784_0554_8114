

using Dal;
using DalApi;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalXml;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependency_xml = "dependencies";

    /// <summary>
    /// Implementation of create function to add a Dependency to the data source
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The id of the new Dependency</returns>
    public int Create(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement newDependency = new XElement("Dependency",
                                                new XElement("ID", Config.NextDependencyId),
                                                new XElement("DependentID", item.DependentID),
                                                new XElement("RequisiteID", item.RequisiteID),
                                                new XElement("Active", item.Active));
                                                

        root.Add(newDependency);
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);
        return item.ID;
    }

    /// <summary>
    /// deletes a dependency
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {

        XElement root = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement? dependency = root.Elements().FirstOrDefault(d => d.Element("ID")!.Value == id.ToString(), null);
        if (dependency == null) throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");

        dependency.Element("Active")!.Value = "false";
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);

    }

    /// <summary>
    /// returns if the entity in question is active
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true is the dependency was not set for deletion, ie, is active</returns>

    public bool isActive(Dependency item)
    {
        return item.Active;
    }

    /// <summary>
    /// Returns a reference to a Dependency object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Dependency if it exists, null otherwise</returns>
    /// /// <exception cref="DalDoesNotExistException"></exception>
    public Dependency? Read(int id)
    {
        IEnumerable<Dependency> dependencies = ReadAll()!;  //only returns the active members

        Dependency? cur = dependencies.FirstOrDefault(i => i.ID == id);

        if (cur == null)
        {
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");
        }

        return cur;
    }


    /// <summary>
    /// read based on a filter argument
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>a dependency that matches the filter argument, default null</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return ReadAll().FirstOrDefault(filter!);
    }

    /// <summary>
    /// Return a copy of all the Dependencies
    /// </summary>
    /// <returns>an IEnumerable of dependencies</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        IEnumerable<Dependency> dependencies = from d in root.Elements()
                                               select new Dependency
                                               {
                                                   ID = int.Parse(d.Element("ID")?.Value!),
                                                   DependentID = int.Parse(d.Element("DependentID")?.Value!),
                                                   RequisiteID = int.Parse(d.Element("RequisiteID")?.Value!),
                                                   Active = bool.Parse(d.Element("Active")?.Value!)

                                               };

        if (filter != null)
        {

            return from d in dependencies
                   where filter(d)
                   where isActive(d) //make sure to only return active items
                   select d;
        }
        return from d in dependencies
               where isActive(d) //make sure to only return active items
               select d;
    }


    /// <summary>
    /// reset the dependencies, set everything to inactive
    /// </summary>
    public void Reset()
    {
        XElement root = new XElement("ArrayOfDependency");
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);

        XElement configRoot = XMLTools.LoadListFromXMLElement("data-config");

        configRoot.Element("NextDependencyID")!.Value = "1";
        XMLTools.SaveListToXMLElement(configRoot, "data-config");

    }


    /// <summary>
    /// Update a Dependency 
    /// </summary>
    /// <param name="item"></param>
    public void Update(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement? dependency = root.Elements().FirstOrDefault(d => d.Element("ID")!.Value == item.ID.ToString(), null);
        if (dependency == null) throw new DalDoesNotExistException($"Dependency with ID={item.ID} does not exist");

        dependency.Element("DependentID")!.Value = item.DependentID.ToString();
        dependency.Element("RequisiteID")!.Value = item.RequisiteID.ToString();
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);
    }
}
