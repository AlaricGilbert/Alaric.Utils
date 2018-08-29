using System;
using System.Runtime.InteropServices;

namespace Alaric.Utils
{
    /// <summary>  
    /// A class provides some useful static methods.
    /// </summary>  
    public static class Utilities
    {
        /// <summary>
        /// The name of the OS.
        /// </summary>
        public static string OSName
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return "Windows";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return "Linux";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return "OSX";
                return "Unknown";
            }
        }

        /// <summary>
        /// Launch a Console window in a WPF/WinForm application.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// Free the Console window launched by AllocConsole Method.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}
