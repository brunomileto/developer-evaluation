namespace Ambev.DeveloperEvaluation.Common.Extensions;

public static class EnumExtensions
{
    public static int Value(this Enum enumValue)
    {
        return Convert.ToInt32(enumValue);
    }

    public static decimal ToPercentage(this Enum enumValue)
    {
        return Convert.ToDecimal(enumValue) / 100;
    }
}
