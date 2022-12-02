using System.Reflection;

namespace Cryptid.Backend
{
    public class VersionManager
    {
        public static string GitHash => Assembly
            .GetEntryAssembly()
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(attr => attr.Key == "GitHash")?.Value;
    }
}
