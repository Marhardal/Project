using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{

    public static class EnumHelper
    {
        public static string GetDisplayName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field.GetCustomAttributes(typeof(DisplayAttribute), false)
                            .Cast<DisplayAttribute>()
                            .FirstOrDefault();

            return attr?.Name ?? value.ToString();
        }
    }
}
