using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NL.Diagnostics {
    public static class SystemInfo {

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SystemAppInfo lpSystemInfo);

        public static SystemAppInfo Retrieve() {
            GetSystemInfo(out SystemAppInfo data);
            return data;
        }
    }

    public struct SystemAppInfo {
        public ushort ProcessorArchitecture;
        public ushort Reserved;
        public uint PageSize;
        public IntPtr MinimumApplicationAddress;
        public IntPtr MaximumApplicationAddress;
        public IntPtr ActiveProcessorMask;
        public uint NumberOfProcessors;
        public uint ProcessorType;
        public uint AllocationGranularity;
        public ushort ProcessorLevel;
        public ushort ProcessorRevision;
    }
}
