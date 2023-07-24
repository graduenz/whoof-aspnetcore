namespace Whoof.Application.Common.Models;

public class ServiceError : IEqualityComparer<ServiceError>
{
    public ServiceError(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Message { get; }
    public int Code { get; }
    
    public static ServiceError DefaultError => new ServiceError(999, "An exception occured.");
    public static ServiceError CustomMessage(string errorMessage) => new ServiceError(998, errorMessage);
    public static ServiceError ForbiddenError => new ServiceError(997, "You are not authorized to call this action.");
    public static ServiceError Cancelled => new ServiceError(996, "The request has been cancelled.");
    public static ServiceError NotFound => new ServiceError(995, "The specified resource was not found.");
    public static ServiceError Validation => new ServiceError(994, "One or more validation errors occurred.");
    public static ServiceError InternalServerError => new ServiceError(500, "Internal server error.");

    public override int GetHashCode()
    {
        return Code;
    }
    
    public override bool Equals(object? obj)
    {
        var error = obj as ServiceError;
        return Equals(this, error);
    }

    public bool Equals(ServiceError? x, ServiceError? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Code == y.Code;
    }

    public int GetHashCode(ServiceError obj)
    {
        return obj.Code;
    }
}