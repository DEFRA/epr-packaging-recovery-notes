using EPRN.Portal.Constants;

namespace EPRN.Portal.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class NamedAttribute : Attribute
    {
        public JourneyType Name { get; }

        public NamedAttribute(JourneyType name)
        {
            Name = name;
        }
    }
}