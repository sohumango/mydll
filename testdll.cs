using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System;

public class test{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class objf{
        int a;
        int b;
        int c;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public UInt32 [] idx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Single [] conf;
        public int r;
        public string nm;
        public int sz1;
        public string nm2;
        public int sz2;
    };

    [DllImport("myDLL.dll")]
    public static extern bool testfunction(int size, out IntPtr ofarr, out int objNum);

    [StructLayout(LayoutKind.Sequential)]
    public struct MyArrayStruct{
        public bool flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] vals;
    }
    [DllImport("myDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int TestArrayInStruct(ref MyArrayStruct myStruct);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MyStruct{
        public string buffer;
        public int size;
    }
    [DllImport("myDLL.dll")]
    public static extern void TestOutArrayOfStructs(out int size, out IntPtr outArray);

    public static void t2(){
        MyArrayStruct myStruct = new MyArrayStruct();
        myStruct.flag = false;
        myStruct.vals = new int[3];
        myStruct.vals[0] = 1;
        myStruct.vals[1] = 4;
        myStruct.vals[2] = 9;
        TestArrayInStruct(ref myStruct);
        Console.WriteLine("\n------t2--------:");
        Console.WriteLine(myStruct.flag);
        Console.WriteLine("{0} {1} {2}", myStruct.vals[0], myStruct.vals[1], myStruct.vals[2]);
    }

    public static void t1(){
        int size;
        IntPtr outArray;
        TestOutArrayOfStructs(out size, out outArray);
        MyStruct[] manArray = new MyStruct[size];
        IntPtr current = outArray;
        Console.WriteLine("\n------t1--------:");
        for (int i = 0; i < size; i++){
            manArray[i] = new MyStruct();
            Marshal.PtrToStructure(current, manArray[i]);
            Marshal.DestroyStructure(current, typeof(MyStruct));
            current = (IntPtr)((long)current + Marshal.SizeOf(manArray[i]));
            Console.WriteLine("Element {0}: {1} {2}", i, manArray[i].buffer,manArray[i].size);
        }
        Marshal.FreeCoTaskMem(outArray);
    }
    static void t3(){
        int size;
        IntPtr outArray;

        Console.WriteLine("\n------t3--------:");
        testfunction( 5, out outArray, out size);

        objf[] farr = new objf[size];
        IntPtr current = outArray;
        Console.WriteLine("count:{0} ",size);
        try{
            for (int i = 0; i < size; i++){
                farr[i] = new objf();
                Marshal.PtrToStructure(current, farr[i]);
                Marshal.DestroyStructure(current, typeof(objf));
                current = (IntPtr)((long)current + Marshal.SizeOf(farr[i]));
                Console.WriteLine("{0}-{1}", i, farr[i].r);
                for (int j = 0; j < 5; j++) {
                    Console.WriteLine("\t{0}-{1}-{2},{3},{4}", j, farr[i].idx[j], farr[i].conf[j],farr[i].nm,farr[i].nm2);
                }
            }
            Marshal.FreeCoTaskMem(outArray);
        } catch(Exception ex) {
            Console.WriteLine(ex);
        }
    }
    public static int Main(string [] argv){
        t1();
        t2();
        t3();
        Console.WriteLine("hello");
        return 0;
    }
}
