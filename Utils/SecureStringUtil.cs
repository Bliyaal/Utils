using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Utils
{
    public class SecureStringUtil
    {
        public static SecureString ToSecureString(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            unsafe
            {
                fixed (char* chars = value)
                {
                    var secureString = new SecureString(chars, value.Length);
                    secureString.MakeReadOnly();
                    return secureString;
                }
            }
        }

        public static string FromSecureString(SecureString secureString)
        {
            if (secureString == null)
            {
                throw new ArgumentNullException(nameof(secureString));
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }

        }

    }
}
