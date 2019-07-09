using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System;

public class test{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public unsafe struct objf{
        public fixed UInt32  idx[5];
        public fixed Single conf[5];
        public int r;
        public fixed byte nm[256];
        public fixed byte nm2[1280];
    };

    [DllImport("myDLL.dll")]public static extern bool testfunction( int size, out IntPtr result, ref int objNum);

    public static int Main(string [] argv){
        IntPtr arr = IntPtr.Zero;
        int objNum = 0;
        testfunction(5, out arr, ref objNum);
        objf []farr = new objf[objNum];
        Console.WriteLine("UInt32 size:{0}, Single size:{1}", sizeof(UInt32),sizeof(Single));
        for (int i = 0; i < objNum; i++){
            int nsize = Marshal.SizeOf(farr[i]) * i; 
            farr[i] = (objf)Marshal.PtrToStructure(arr+nsize, typeof(objf));    
        }
        for (int i = 0; i < objNum; i++){
            Console.WriteLine("{0}-{1}", i, farr[i].r);
            for (int j = 0; j < 5; j++) {
                unsafe{
                    Console.WriteLine("\t\t{0}-{1}-{2}", j, farr[i].idx[j], farr[i].conf[j]);
                }
            }
        }
        Console.WriteLine("hello");
        return 0;
    }
}
