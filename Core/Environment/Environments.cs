namespace Core.Environment
{
    public static class Environments
    {
        public static readonly string Local = Microsoft.Extensions.Hosting.Environments.Development;
        public static readonly string Staging = Microsoft.Extensions.Hosting.Environments.Staging;
        public static readonly string Production = Microsoft.Extensions.Hosting.Environments.Production;

        public static readonly string Development = "Dev";
        public static readonly string Test = "Test";
        public const string Demo = "Demo";
    }
}