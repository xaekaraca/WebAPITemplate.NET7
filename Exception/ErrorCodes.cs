namespace Exception;

public abstract class ErrorCodes
{
    public const string SystemInternalServerError = "System.Internal.Server.Error";
    public const string DatabaseError = "System.Database.Error";
    public const string OperationalError = "System.Operational.Error";
    public const string UnauthorizedError = "System.Unauthorized.Error";
    public const string EmailCannotBeEmptyError = "System.Email.Cannot.Be.Empty.Error";
    public const string LoginProviderCannotBeEmptyError = "System.Login.Provider.Cannot.Be.Empty.Error";
    public const string InvalidTokenError = "System.Invalid.Token.Error";
    public const string InvalidAgentError = "System.Invalid.Agent.Error";

    public const string BusinessInternalServerError = "Business.Internal.Server.Error";
    public const string NotFoundError = "Business.Not.Found.Error";
    public const string NullError = "Business.Null.Error";
    public const string ForbiddenError = "Business.Forbidden.Error";
    public const string AlreadyExistsError = "Business.Already.Exists.Error";
}