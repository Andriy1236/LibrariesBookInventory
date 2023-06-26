using System.IO;

namespace LibrariesBookInventory.Persistence.Migrations.Scripting
{
    public static class MigrationExtensions
    {
        private static readonly ScriptProvider _scripting = new ScriptProvider();

        public static string ScriptFromFile(string resourceName)
        {
            using (var stream = _scripting.FindScriptResource(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string script = reader.ReadToEnd();
                return script;
            }
        }
    }
}
