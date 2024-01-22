

namespace DalXml;

using Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskID"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyID"); }

    internal static DateTime? ProjectStart { get => XMLTools.GetProjectDate(s_data_config_xml, "ProjectStart"); set => XMLTools.SetProjectDate(s_data_config_xml, "ProjectStart", value!.Value); }

    internal static DateTime? ProjectEnd { get => XMLTools.GetProjectDate(s_data_config_xml, "ProjectEnd"); set => XMLTools.SetProjectDate(s_data_config_xml, "ProjectEnd", value!.Value); }

}