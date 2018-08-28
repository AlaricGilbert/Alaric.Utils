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
        /// Get the String represents the enum type element.
        /// </summary>
        /// <returns>The string.</returns>
        public static string EnumToString<TEnum>(int value)
        {
            return Enum.GetName(typeof(TEnum), value);
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
