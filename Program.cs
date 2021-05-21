using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Memory;
using LiveSplit.ComponentUtil;
using Binarysharp.MemoryManagement;

namespace cotdg_mod
{
    class Program
    {

        public async void FindPlayer ()
        {
            Process process = Process.GetProcessesByName("Curse of the Dead Gods")[0];
            Console.WriteLine("Test");
            Mem MemLib = new Mem();
            // open the process and check if it was successful before the AoB scan
            if (!MemLib.OpenProcess("Curse of the Dead Gods")) // you can also specify the process ID. Check Wiki for more info.
            {
                Console.WriteLine("Process Is Not Found or Open!");
                return;
            }

            // AoB scan and store it in AoBScanResults. We specify our start and end address regions to decrease scan time.
            IEnumerable<long> AoBScanResults = await MemLib.AoBScan("F3 0F 10 B3 10 EE 00 00 74");

            // get the first found address, store it in the variable SingleAoBScanResult
            long SingleAoBScanResult = AoBScanResults.FirstOrDefault();
            Console.WriteLine(MemLib.ReadLong((SingleAoBScanResult).ToString("X")).ToString("X"));
            //Console.WriteLine(MemLib.ReadUInt(SingleAoBScanResult.ToString());
            // pop up message box that shows our first result
            Console.WriteLine("Our First Found Address is " + SingleAoBScanResult);

            var address = IntPtr.Zero;
            var sharp = new MemorySharp(process);

            // Execute code and get the return value as boolean
            sharp.Assembly.Inject(
            new[]
                {
                    "mov Curse of the Dead Gods.exe+0,rbx",
                    "movss xmm6,[rbx+0000EE10]",
                    "jmp return"
                },
            address);
                        
            long SinglePlusOffsets = SingleAoBScanResult + 0x174 + 0x860 + 0x20 + 0x08;
            Console.WriteLine((SingleAoBScanResult).ToString("X"));
            Console.WriteLine((SinglePlusOffsets).ToString("X"));
           // var hp = MemLib.ReadFloat("Curse of the Dead Gods.exe" + (SingleAoBScanResult + 0x174 + 0x860 + 0x20 + 0x08).ToString("X"));

            //// Ex: iterate through each found address. This prints each address in the debug console in Visual Studio.
            //foreach (long res in AoBScanResults)
            //{
            //    Debug.WriteLine("I found the address {0} in the AoB scan.", res, null);
            //}

            //// Ex: read the value from our first found address, convert it to a string, and show a pop up message - https://github.com/erfg12/memory.dll/wiki/Read-Memory-Functions
            //Console.WriteLine("Value for our address is " + MemLib.ReadFloat(SingleAoBScanResult.ToString("X")).ToString());

            //// Ex: write to our first found address - https://github.com/erfg12/memory.dll/wiki/writeMemory
            //MemLib.WriteMemory(SingleAoBScanResult.ToString("X"), "float", "100.0");
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            
            p.FindPlayer();
            Thread.Sleep(4000);
            //Uses dll library to scan for an array of bytes 
            //Scanner Obj = new Scanner(process, process.Handle, "F3 0F 10 B3 10 EE 00 00 80");
            //Obj.setModule(process.MainModule);
            //IntPtr playerPointer = (IntPtr)Obj.FindPattern();
            //playerPointer += 0x174 + 0x860 + 0x20 + 0x08;
            //IntPtr processHandle = OpenProcess((IntPtr)PROCESS_WM_READ, false, (IntPtr)process.Id); 
            //int bytesRead;
            //byte[] buffer = new byte[32]; //To read a 24 byte unicode string

            //var test = ReadProcessMemory((IntPtr)processHandle, playerPointer, buffer, buffer.Length, out bytesRead);
            //Console.WriteLine(playerPointer);
            ////Uses code from livesplit.component to get values from pointers
            ////playerPointer = (IntPtr)0x5BDEF5;
            ////int[] offsets = {0x174, 0x860, 0x20, 0x08};
            ////IntPtr ptr = process.MainModuleWow64Safe().BaseAddress + _base;
            ////IntPtr bufPtr = new IntPtr(playerPointer);

            //Console.WriteLine(Encoding.Unicode.GetString(buffer) + " (" + bytesRead.ToString() + "bytes)");
            //Console.ReadLine();
            //DeepPointer IGT = new DeepPointer(playerPointer, offsets);
            //Console.WriteLine(playerPointer.ToString("X"));
            //Console.WriteLine(GetValue(process, "int", IGT));
            // IntPtr writer;
            // IGT.DerefOffsets(process, out writer);
            // process.WriteValue(writer, 100);



            //         public static void Main()
            //         {
            //             Process process = Process.GetProcessesByName("Curse of the Dead Gods")[0];
            //             97978XElement pointers = XElement.Load(@"..\..\..\pointers.xml");
            //             Console.WriteLine(pointers.Elements("Pointer"));
            //             int[] offsets = {0xD8, 0x1150};
            //             OffsetT _base = 0x01206F30;
            //             IntPtr ptr = process.MainModuleWow64Safe().BaseAddress + _base;
            //             int[] dex_offsets = {0xA4, 0x18, 0x498};
            //             DeepPointer IGT = new DeepPointer(DEXTERITY_BASE, dex_offsets);
            //             Console.WriteLine(GetValue(process, "int", IGT));
            //             IntPtr writer;
            //             IGT.DerefOffsets(process, out writer);
            //             process.WriteValue(writer, 100);
            //         }
        }
    }
}


// This code works for the current version of the game, it can find and modify pointers, unfortunatly i think the important pointers move every single room so i need to aobscan every room (1.24.3.something)

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System.Diagnostics;
// using System.Runtime.InteropServices;
// using LiveSplit.ComponentUtil;
// using System.Xml.Linq;




// namespace cotdg_mod
// {
//     using OffsetT = Int32;
//     class Program {

//         const OffsetT DEXTERITY_BASE = 0x01206F30;
//         int[] DEXTERITY_OFFSETS = {0xA4, 0x18, 0x498};
//         const int PROCESS_WM_READ = 0x0010;

//         [DllImport("kernel32.dll")]
//         public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

//         [DllImport("kernel32.dll")]
//         public static extern bool ReadProcessMemory(int hProcess, 
//         int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        

//         public static void Main()
//         {
//             Process process = Process.GetProcessesByName("Curse of the Dead Gods")[0];
//             97978XElement pointers = XElement.Load(@"..\..\..\pointers.xml");
//             Console.WriteLine(pointers.Elements("Pointer"));
//             int[] offsets = {0xD8, 0x1150};
//             OffsetT _base = 0x01206F30;
//             IntPtr ptr = process.MainModuleWow64Safe().BaseAddress + _base;
//             int[] dex_offsets = {0xA4, 0x18, 0x498};
//             DeepPointer IGT = new DeepPointer(DEXTERITY_BASE, dex_offsets);
//             Console.WriteLine(GetValue(process, "int", IGT));
//             IntPtr writer;
//             IGT.DerefOffsets(process, out writer);
//             process.WriteValue(writer, 100);
//         }
//     }
// }


