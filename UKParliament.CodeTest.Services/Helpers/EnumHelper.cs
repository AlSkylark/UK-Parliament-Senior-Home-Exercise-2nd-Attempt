using System.ComponentModel;

namespace UKParliament.CodeTest.Services.Helpers;

public static class EnumHelper
{
    public static IEnumerable<string> GetNameValues(this Type enumType)
    {
        if (!enumType.IsEnum)
        {
            return [];
        }

        var values = Enum.GetValues(enumType);
        List<string> names = [];
        foreach (var name in values)
        {
            names.Add(name.ToString() ?? "");
        }

        return names;
    }

    public static string GetDescription(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (field == null)
            return enumValue.ToString();

        if (
            Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
            is DescriptionAttribute attribute
        )
        {
            return attribute.Description;
        }

        return enumValue.ToString();
    }

    public static bool IsDefinedInEnum(this string value, Type enumType)
    {
        var enumVal = Enum.Parse(enumType, value, true);
        if (enumVal is null)
        {
            return false;
        }

        return Enum.IsDefined(enumType, enumVal);
    }
}
