using System;
using System.IO;
using System.Reflection;

namespace ClientManagedKeys.Server
{
    public static class AssemblyExtensions
    {
        public static byte[] GetResourceAsBytes(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + "." + resourceName))
            {
                if (stream == null)
                    throw new Exception($"Resource {resourceName} not found in assembly {assembly.GetName()}");

                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"Failed to read resource {resourceName} not found in assembly {assembly.GetName()}", e);
                }
            }
        }
    }
}