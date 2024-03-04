



namespace PL
{

    using System.Collections;

    internal class ExperienceCollection : IEnumerable
    {
        static readonly IEnumerable<BO.EngineerExperience> s_enums =
       (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();


    }

    internal class ExperienceCollectionForEntities : IEnumerable
    {
        static readonly IEnumerable<BO.EngineerExperience> s_enums =
        ((Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!)
        .SkipLast(1); // Skip the last value

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
