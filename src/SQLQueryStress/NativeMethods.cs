using System.Runtime.InteropServices;

namespace SQLQueryStress
{
    internal static class NativeMethods
    {
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        private static extern bool AttachConsole(int processId);

        internal static bool AttachParentConsole() => AttachConsole(ATTACH_PARENT_PROCESS);
    }
}