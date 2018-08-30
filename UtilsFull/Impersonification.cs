using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Utils
{
    public class Impersonification
    {
        [DllImport("advapi32.dll", SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LogonUser(
           [MarshalAs(UnmanagedType.LPStr)] string pszUserName,
           [MarshalAs(UnmanagedType.LPStr)] string pszDomain,
           [MarshalAs(UnmanagedType.LPStr)] string pszPassword,
           int dwLogonType,
           int dwLogonProvider,
           ref IntPtr phToken);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        private const int Logon32LogonInteractive = 2;
        private const int Logon32ProviderDefault = 0;
        private const string TypeAuthentification = "NTLM";

        public static WindowsImpersonationContext ElevateToLocalAdmin()
        {
            WindowsImpersonationContext impersonateContext = null;
            IntPtr token = GetLocalAdminToken();

            if (token != IntPtr.Zero)
            {
                var identity = new WindowsIdentity(token, TypeAuthentification, WindowsAccountType.Normal, true);
                Thread.CurrentPrincipal = new WindowsPrincipal(identity);
                impersonateContext = identity.Impersonate();
                CloseHandle(token);
            }

            if (impersonateContext == null)
            {
                throw new Exception();
            }

            return impersonateContext;
        }

        private static IntPtr GetLocalAdminToken()
        {
            IntPtr token = IntPtr.Zero;

            bool resultat = LogonUser(Environment.UserName,
                                      Environment.MachineName,
                                      SecureStringUtil.FromSecureString(GetPassword()),
                                      Logon32LogonInteractive,
                                      Logon32ProviderDefault,
                                      ref token);

            if (resultat)
            {
                return token;
            }

            return IntPtr.Zero;
        }

        private static SecureString GetPassword()
        {
            if (!File.Exists(@"C:\users.dat"))
            {
                throw new InvalidOperationException("users.dat not found");
            }

            byte[] data = File.ReadAllBytes(@"C:\users.dat");
            data = ProtectedData.Unprotect(data,
                                           new byte[] { 1, 0, 7, 4, 4, 7, 2, 5 },
                                           DataProtectionScope.LocalMachine);

            string[] credentials = Encoding.UTF8.GetString(data).Split(';');
            if (!credentials.Any((a) => a.Contains(Environment.UserName)))
            {
                throw new IdentityNotMappedException($"User {Environment.UserName} not found");
            }

            return SecureStringUtil.ToSecureString(credentials.First(a => a.Contains(Environment.UserName))
                                                              .Split('=')[1]);
        }
    }
}
