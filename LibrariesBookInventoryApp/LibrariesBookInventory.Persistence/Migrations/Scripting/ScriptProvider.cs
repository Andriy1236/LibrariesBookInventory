using System;
using System.IO;
using System.Linq;

namespace LibrariesBookInventory.Persistence.Migrations.Scripting
{
    public class ScriptProvider
    {
        public Stream FindScriptResource(string resourceName)
        {
            var resourceAssembly = typeof(ScriptProvider)
                .Assembly;

            var fullResourceName = $"{resourceName}.sql";

            var resources = resourceAssembly
                .GetManifestResourceNames()
                .Where(n => n.EndsWith(fullResourceName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (resources.Count == 0)
            {
                if (resourceName.Contains("."))
                {
                    throw new InvalidOperationException($"Couldn't find resource {fullResourceName}. FindScriptResource expects the file name without the extensions. Make sure your migration is passing 'My_Script', not 'My_Script.sql'.");
                }

                throw new FileNotFoundException($"Couldn't find resource {fullResourceName} anywhere in the {resourceAssembly.GetName().Name} assembly.", fullResourceName);
            }

            if (resources.Count > 1)
            {
                throw new InvalidOperationException($"Found more than one ({resources.Count}) {fullResourceName} resources in the {resourceAssembly.GetName().Name} assembly.");
            }

            return resourceAssembly.GetManifestResourceStream(resources[0]);
        }
    }
}
