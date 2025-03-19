using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RavistekTicket.Peresentation.PresentationExtensions
{
    public static class EnumExtentions
    {
        public static string GetEnumName(this System.Enum eEnum)
        {
            var enumName = eEnum.GetType()
                .GetMember(eEnum.ToString())
                .FirstOrDefault();

            if (enumName != null)
            {
                return enumName.GetCustomAttribute<DisplayAttribute>().GetName();
            }

            return "";
        }
    }
}
