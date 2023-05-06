using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Exception;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public abstract class Exception : System.Exception
{
    protected Exception(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected Exception(string code, string level, System.Exception? innerException = null) : base(null, innerException)
    {
        Code = code;
        Level = level;
    }

    public string Code { get; } = null!;

    public string Level { get; } = null!;
}

[Serializable]
public class SystemException : Exception
{
    protected SystemException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public SystemException(string code, System.Exception? innerException = null) : base(code, ErrorLevels.System, innerException)
    {
    }
}

[Serializable]
public class DatabaseException : SystemException
{
    protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DatabaseException(System.Exception? innerException = null) : base(ErrorCodes.DatabaseError, innerException)
    {
    }
}

[Serializable]
public class OperationalException : SystemException
{
    protected OperationalException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public OperationalException(System.Exception? innerException = null) : base(ErrorCodes.OperationalError, innerException)
    {
    }

    public OperationalException(string errorCode, System.Exception? innerException = null) : base(errorCode, innerException)
    {
    }
}

[Serializable]
public class UnauthorizedException : SystemException
{
    protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UnauthorizedException(System.Exception? innerException = null) : base(ErrorCodes.UnauthorizedError, innerException)
    {
    }
}

[Serializable]
public class BusinessException : Exception
{
    protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BusinessException(string code, System.Exception? innerException = null) : base(code, ErrorLevels.Business, innerException)
    {
    }
}

[Serializable]
public class ForbiddenException : BusinessException
{
    protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ForbiddenException(System.Exception? innerException = null) : base(ErrorCodes.ForbiddenError, innerException)
    {
    }
}

[Serializable]
public class NotFoundException : BusinessException
{
    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NotFoundException(System.Exception? innerException = null) : base(ErrorCodes.NotFoundError, innerException)
    {
    }
}

[Serializable]
public class NullException : BusinessException
{
    protected NullException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NullException(System.Exception? innerException = null) : base(ErrorCodes.NullError, innerException)
    {
    }
}

[Serializable]
public class AlreadyExistsException : BusinessException
{
    protected AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public AlreadyExistsException(System.Exception? innerException = null) : base(ErrorCodes.AlreadyExistsError, innerException)
    {
    }
}