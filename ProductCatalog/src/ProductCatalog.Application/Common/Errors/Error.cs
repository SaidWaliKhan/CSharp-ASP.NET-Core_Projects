namespace ProductCatalog.Application.Common.Errors;
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NotFound = new("General.NotFound", "Resource not found.");

    public static readonly Error NullValue = new("General.NullValue", "A null value was provided.");
}