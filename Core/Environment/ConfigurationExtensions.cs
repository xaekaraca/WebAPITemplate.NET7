using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Core.Environment
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class ConfigurationExtensions
    {
        private const string AspnetcoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        private static string? Environment(this IConfiguration configuration)
        {
            return configuration[AspnetcoreEnvironment];
        }

        public static bool IsSensitiveEnvironment(this IConfiguration configuration)
        {
            return configuration.IsProductionEnvironment() || configuration.IsStagingEnvironment() || configuration.IsDemoEnvironment();
        }

        public static bool IsProductionEnvironment(this IConfiguration configuration)
        {
            return string.Equals(configuration.Environment(), Environments.Production, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsStagingEnvironment(this IConfiguration configuration)
        {
            return string.Equals(configuration.Environment(), Environments.Staging, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsDemoEnvironment(this IConfiguration configuration)
        {
            return string.Equals(configuration.Environment(), Environments.Demo, StringComparison.OrdinalIgnoreCase);
        }
    }
}