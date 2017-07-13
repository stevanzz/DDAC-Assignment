using Events.Models;

namespace Events.Web.Helpers
{
    public static class EnumHelpers
    {
        public static string GetPluralizedName(this ContainerType me)
        {
            switch (me)
            {
                case ContainerType.Standard1:
                    return "20' Dry Standard";

                case ContainerType.Standard2:
                    return "40' Dry Standard";

                case ContainerType.High1:
                    return "40' Dry High";

                case ContainerType.High2:
                    return "45' Dry High";

                default:
                    return string.Empty;
            }
        }
    }
}