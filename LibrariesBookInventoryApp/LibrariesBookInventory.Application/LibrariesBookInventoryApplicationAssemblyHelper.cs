using System.Reflection;

namespace LibrariesBookInventory.Application
{
    public class LibrariesBookInventoryApplicationAssemblyHelper
    {
        public static Assembly GetLogicAssembly() => typeof(LibrariesBookInventoryApplicationAssemblyHelper).Assembly;

    }
}
