using NL.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace NL.Utils.Processes {
    public class ProcessMemory : IDisposable {
        #region Extern
        [DllImport("kernel32.dll")]
        private static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        private static extern int GetLastError();

        private const uint DELETE = 0x00010000;
        private const uint READ_CONTROL = 0x00020000;
        private const uint WRITE_DAC = 0x00040000;
        private const uint WRITE_OWNER = 0x00080000;
        private const uint SYNCHRONIZE = 0x00100000;
        private const uint PROCESS_QUERY_INFORMATION = 0x0400;
        private const uint MEM_COMMIT = 0x00001000;
        private const uint PAGE_READWRITE = 0x04;
        private const uint PROCESS_WM_READ = 0x0010;
        private const uint END = 0xFFF; //Should be 0xFFFF for Windows XP & Windows Server 2003
        private const uint PROCESS_ALL_ACCESS = DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER | SYNCHRONIZE | PROCESS_QUERY_INFORMATION | PAGE_READWRITE | MEM_COMMIT | PROCESS_WM_READ | END;
        #endregion

        public Process Process { get; private set; }
        public SystemAppInfo System { get; private set; }
        private readonly int _processHandle;
        private IntPtr _minimumAddress => System.MinimumApplicationAddress;
        private IntPtr _maximumAddress => System.MaximumApplicationAddress;
        public long MinimumAddress => (long)_minimumAddress;
        public long MaximumAddress => (long)_maximumAddress;

        #region Constructors
        public ProcessMemory(string processName) : this(Process.GetProcessesByName(processName)[0]) { }
        public ProcessMemory(int processId) : this(Process.GetProcessById(processId)) { }
        public ProcessMemory(Process process) {
            Process = process;
            System = SystemInfo.Retrieve();
            _processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, Process.Id);
        }
        #endregion

        public byte[] Read(int startBlockNumber, int blockSize) {
            byte[] buffer = new byte[blockSize];
            IntPtr startingBlock = _minimumAddress + startBlockNumber;
            ReadProcessMemory(_processHandle, (int)startingBlock, buffer, blockSize, 0);
            return buffer;
        }

        public bool TryRead(int startBlockNumber, int blockSize, out byte[] buffer, out int errorCode) {
            buffer = new byte[blockSize];
            IntPtr startingBlock = _minimumAddress + startBlockNumber;
            if(ReadProcessMemory(_processHandle, (int)startingBlock, buffer, blockSize, 0)) {
                errorCode = 0;
                return true;
            }else {
                errorCode = GetLastError();
                return false;
            }
        }

        public void Write(int address, byte[] processData) {
            WriteProcessMemory(_processHandle, address, processData, processData.Length, 0);
        }

        public int GetObjectSize(object obj) {
            BinaryFormatter formatter = new();
            MemoryStream stream = new();

            formatter.Serialize(stream, obj);
            return stream.ToArray().Length;
        }

        public void Dispose() {
            Process.Dispose();
            Process = null;
        }
    }

    public struct MemoryData {
        public int BaseAddress;
        public int AllocationBase;
        public int AllocationProtect;
        public int RegionSize;
        public int State;
        public int Protect;
        public int LType;
    }
}
