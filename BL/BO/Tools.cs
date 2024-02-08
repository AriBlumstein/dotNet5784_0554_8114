



namespace BO;
using System.Reflection;


public static class Tools
{
    public static String ToStringProperty<T>(this T t, int indentationLevel=0)
    {
        String indentString = string.Concat(Enumerable.Repeat("\t", indentationLevel));
        String str = "";

        str+=t.GetType().Name+":";


        foreach (var item in t.GetType().GetProperties())
        {

            if (item.PropertyType == typeof(List<TaskInList>))
            {
                var list = (List<TaskInList>)item.GetValue(t);

                if (list == null)
                {
                    continue;
                }

                str += "\n" + item.Name +":";

                foreach (var listItem in list)
                {
                    str += "\n" + listItem.ToStringProperty(indentationLevel + 1);
                }
            }
            else if ((item.PropertyType == typeof(BO.MilestoneInTask) || item.PropertyType == typeof(BO.EngineerInTask) || item.PropertyType == typeof(BO.TaskInEngineer)) && item.GetValue(t) != null)
            {
                str += "\n" + item.Name + ":" + "\n\t" + item.GetValue(t).ToStringProperty(indentationLevel + 1);
            }
            else
            {
                str += "\n" + indentString + item.Name + ": " + item.GetValue(t, null);
            }
            


        }
        return str+"\n";
    }
}
