namespace Pb.ApiGateway.Extensions;

public static class ObjectExtensions
{
    public static bool IsAnyNullOrEmpty(this object obj)
    {
        if (ReferenceEquals(obj, null))
            return true;

        return obj.GetType().GetProperties()
            .Any(x => IsNullOrEmpty(x.GetValue(obj)));
    }
    
    private static bool IsNullOrEmpty(object? value)
    {
        if (ReferenceEquals(value, null))
            return true;

        var type = value.GetType();
        return type.IsValueType
               && Equals(value, Activator.CreateInstance(type));
    }
}