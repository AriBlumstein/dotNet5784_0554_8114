

using Dal;
using DalApi;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalXml;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependency_xml = "dependencies";
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

    public void Delete(int id)
    {

        XElement root = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement? dependency = root.Elements().FirstOrDefault(d => d.Element("ID")!.Value == id.ToString(), null);
        if (dependency == null) throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");

        dependency.Element("Active")!.Value = "false";
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);

    }

    public bool isActive(Dependency item)
    {
        return item.Active;
    }

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

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return ReadAll().FirstOrDefault(filter!);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        IEnumerable<Dependency> dependencies = ReadXElement();

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

    private IEnumerable<Dependency> ReadXElement()
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
        return dependencies;
    }

    public void Reset()
    {
        XElement root = new XElement("ArrayOfDependency");
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);

        XElement configRoot = XMLTools.LoadListFromXMLElement("data-config");

        configRoot.Element("NextDependencyID")!.Value = "1";
        XMLTools.SaveListToXMLElement(configRoot, "data-config");

    }

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
