namespace Whoof.Application.Common.Models;

public class ServiceError
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
    
    public override bool Equals(object? obj)
    {
        var error = obj as ServiceError;

        return Code == error?.Code;
    }

    public bool Equals(ServiceError error)
    {
        return Code == error?.Code;
    }

    public override int GetHashCode()
    {
        return Code;
    }

    public static bool operator ==(ServiceError? a, ServiceError? b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if ((object?)a == null || (object?)b == null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ServiceError? a, ServiceError? b)
    {
        return !(a == b);
    }
}