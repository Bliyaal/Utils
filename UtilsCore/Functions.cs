using System;
using System.Diagnostics;
using System.Reflection;

namespace Utils
{
    public sealed class Functions
    {
        /// <summary>
        /// Stops the execution and deletes the calling assembly it after delay has expired.
        /// </summary>
        /// <param name="delay">Delay in seconds before execution of the delete command. Must be between 0 and 999999</param>
        /// <remarks>A too short delay can prevent the calling assembly to be closed when the delete command is executed.</remarks>
        public static void SelfDeleteAssembly(int delay)
        {
            if (delay < 0 ||
                delay > 99999)
            {
                throw new ArgumentOutOfRangeException(nameof(delay));
            }
            var cmd = new ProcessStartInfo("cmd.exe",
                                           $"/c timeout /t {delay} /nobreak & del \"{Assembly.GetCallingAssembly().Location}\"")
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(cmd);
            Environment.Exit(0);
        }

        /// <summary>
        /// Repeat a function call until the function returns the right condition.
        /// </summary>
        /// <param name="function">Delegate for the function. The function must returns a bool.</param>
        /// <param name="condition">The call will be repeated as long as the function returns condition</param>
        public static void RepeatWhile(Func<bool> function,
                                       bool condition)
        {
            while (function() == condition) { }
        }
    }
}
