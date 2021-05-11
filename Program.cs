using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LiveSplit.ComponentUtil;



namespace cotdg_mod
{
    using OffsetT = Int32;
    class Program {

        
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, 
        int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        
        private static dynamic GetValue(Process p, string type, DeepPointer pointer)
        {
            switch (type)
            {
                case "int":
                    return pointer.Deref<int>(p);
                case "uint":
                    return pointer.Deref<uint>(p);
                case "long":
                    return pointer.Deref<long>(p);
                case "ulong":
                    return pointer.Deref<ulong>(p);
                case "float":
                    return pointer.Deref<float>(p);
                case "double":
                    return pointer.Deref<double>(p);
                case "byte":
                    return pointer.Deref<byte>(p);
                case "sbyte":
                    return pointer.Deref<sbyte>(p);
                case "short":
                    return pointer.Deref<short>(p);
                case "ushort":
                    return pointer.Deref<ushort>(p);
                case "bool":
                    return pointer.Deref<bool>(p);
                default:
                    if (type.StartsWith("string"))
                    {
                        var length = int.Parse(type.Substring("string".Length));
                        return pointer.DerefString(p, length);
                    }
                    else if (type.StartsWith("byte"))
                    {
                        var length = int.Parse(type.Substring("byte".Length));
                        return pointer.DerefBytes(p, length);
                    }
                    break;
            }

            throw new ArgumentException($"The provided type, '{type}', is not supported");
        }
        public static void Main()
        {
            Process process = Process.GetProcessesByName("Curse of the Dead Gods")[0];
            int[] offsets = {0xD8, 0x1150};
            OffsetT _base = 0x01206F30;
            IntPtr ptr = process.MainModuleWow64Safe().BaseAddress + _base;
            DeepPointer IGT = new DeepPointer(0x01206F30, offsets);
            Console.WriteLine(GetValue(process, "int", IGT));
            IntPtr writer;
            IGT.DerefOffsets(process, out writer);
            process.WriteValue(writer, 100);
        }
    }
}
