using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace LaborSafety.Utils.Security
{
    public static class ImpersonateUtils
    {
        private const int SW_SHOW = 5;
        private const int TOKEN_QUERY = 0x0008;
        private const int TOKEN_DUPLICATE = 0x0002;
        private const int TOKEN_ASSIGN_PRIMARY = 0x0001;
        private const int STARTF_USESHOWWINDOW = 0x00000001;
        private const int STARTF_FORCEONFEEDBACK = 0x00000040;
        private const int CREATE_UNICODE_ENVIRONMENT = 0x00000400;
        private const int TOKEN_IMPERSONATE = 0x0004;
        private const int TOKEN_QUERY_SOURCE = 0x0010;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private const int TOKEN_ADJUST_GROUPS = 0x0040;
        private const int TOKEN_ADJUST_DEFAULT = 0x0080;
        private const int TOKEN_ADJUST_SESSIONID = 0x0100;
        private const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        private const int TOKEN_ALL_ACCESS =
            STANDARD_RIGHTS_REQUIRED |
            TOKEN_ASSIGN_PRIMARY |
            TOKEN_DUPLICATE |
            TOKEN_IMPERSONATE |
            TOKEN_QUERY |
            TOKEN_QUERY_SOURCE |
            TOKEN_ADJUST_PRIVILEGES |
            TOKEN_ADJUST_GROUPS |
            TOKEN_ADJUST_DEFAULT |
            TOKEN_ADJUST_SESSIONID;

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        private enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        private enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            int dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            int ImpersonationLevel,
            int dwTokenType,
            ref IntPtr phNewToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            int DesiredAccess,
            ref IntPtr TokenHandle);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool CreateEnvironmentBlock(
                ref IntPtr lpEnvironment,
                IntPtr hToken,
                bool bInherit);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool DestroyEnvironmentBlock(
                IntPtr lpEnvironment);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(
            IntPtr hObject);

        private static void LaunchProcessAsUser(string cmdLine, IntPtr token, IntPtr envBlock, int sessionId)
        {
            var pi = new PROCESS_INFORMATION();
            var saProcess = new SECURITY_ATTRIBUTES();
            var saThread = new SECURITY_ATTRIBUTES();
            saProcess.nLength = Marshal.SizeOf(saProcess);
            saThread.nLength = Marshal.SizeOf(saThread);

            var si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            si.lpDesktop = @"WinSta0\Default";
            si.dwFlags = STARTF_USESHOWWINDOW | STARTF_FORCEONFEEDBACK;
            si.wShowWindow = SW_SHOW;

            if (!CreateProcessAsUser(
                token,
                null,
                cmdLine,
                ref saProcess,
                ref saThread,
                false,
                CREATE_UNICODE_ENVIRONMENT,
                envBlock,
                null,
                ref si,
                out pi))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateProcessAsUser failed");
            }
        }

        private static IDisposable Impersonate(IntPtr token)
        {
            var identity = new WindowsIdentity(token);
            return identity.Impersonate();
        }

        private static IntPtr GetPrimaryToken(Process process)
        {
            var token = IntPtr.Zero;
            var primaryToken = IntPtr.Zero;

            if (OpenProcessToken(process.Handle, TOKEN_DUPLICATE, ref token))
            {
                var sa = new SECURITY_ATTRIBUTES();
                sa.nLength = Marshal.SizeOf(sa);

                if (!DuplicateTokenEx(
                    token,
                    TOKEN_ALL_ACCESS,
                    ref sa,
                    (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation,
                    (int)TOKEN_TYPE.TokenPrimary,
                    ref primaryToken))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "DuplicateTokenEx failed");
                }

                CloseHandle(token);
            }
            else
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "OpenProcessToken failed");
            }

            return primaryToken;
        }

        private static IntPtr GetEnvironmentBlock(IntPtr token)
        {
            var envBlock = IntPtr.Zero;
            if (!CreateEnvironmentBlock(ref envBlock, token, false))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateEnvironmentBlock failed");
            }
            return envBlock;
        }

        public static void LaunchAsCurrentUser(string cmdLine)
        {
            var process = Process.GetProcessesByName("explorer").FirstOrDefault();
            if (process != null)
            {
                var token = GetPrimaryToken(process);
                if (token != IntPtr.Zero)
                {
                    var envBlock = GetEnvironmentBlock(token);
                    if (envBlock != IntPtr.Zero)
                    {
                        LaunchProcessAsUser(cmdLine, token, envBlock, process.SessionId);
                        if (!DestroyEnvironmentBlock(envBlock))
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error(), "DestroyEnvironmentBlock failed");
                        }
                    }

                    CloseHandle(token);
                }
            }
        }

        public static IDisposable ImpersonateCurrentUser()
        {
            var process = Process.GetProcessesByName("explorer").FirstOrDefault();
            if (process != null)
            {
                var token = GetPrimaryToken(process);
                if (token != IntPtr.Zero)
                {
                    return Impersonate(token);
                }
            }

            throw new Exception("Could not find explorer.exe");
        }
    }
}
