using System;
using System.Collections.Generic;
namespace AuthServer.Packets
{
    public unsafe class Writer
    {
        public byte[] Bytes;
        public Writer(int size) { this.Bytes = new byte[size]; }
        public Writer() { }
        public void Fill(byte value, int offset)
        {
            fixed (byte* Packet = Bytes)
                *(byte*)(Packet + offset) = value;
        }
        public void Fill(int value, int offset)
        {
            fixed (byte* Packet = Bytes)
                *(int*)(Packet + offset) = value;
        }
        public void Fill(ushort value, int offset)
        {
            fixed (byte* Packet = Bytes)
                *(ushort*)(Packet + offset) = value;
        }
        public void Fill(uint value, int offset)
        {
            fixed (byte* Packet = Bytes)
                *(uint*)(Packet + offset) = value;
        }
        public void Fill(ulong value, int offset)
        {
            fixed (byte* Packet = Bytes)
                *(ulong*)(Packet + offset) = value;
        }
        public void Fill(string value, int offset)
        {
            fixed (byte* Packet = Bytes)
                Encode(Packet, value, offset);
        }
        public void Fill(string value, byte size, int offset)
        {
            fixed (byte* Packet = Bytes)
            {
                *(byte*)(Packet + offset) = size;
                offset++;
                Encode(Packet, value, offset);
            }
        }
        public void Fill(List<string> Values, int offset)
        {
            if (Values == null)
                return;
            if (offset > Bytes.Length - 1)
                return;
            Bytes[offset] = (byte)Values.Count;
            offset++;
            foreach (string str in Values)
            {
                Bytes[offset] = (byte)str.Length;
                Fill(str, offset + 1);
                offset += str.Length + 1;
            }
        }
        public static void Encode(byte* Packet, string Str, int Index)
        {
            fixed (char* Ptr = Str)
                Copy(Ptr, Packet, Index, 0, Str.Length);
        }
        public static unsafe void Copy(char* pSrc, byte* pDst, int dstIndex, int srcIndex, int Count)
        {
            byte* ps = ((byte*)(pSrc + srcIndex)), pd = (pDst + dstIndex);
            for (int i = 0; i < Count; i++)
            {
                *pd = *ps;
                pd++;
                ps += 2;
            }
        }
        public static byte[] Fill(uint Value, int Offset, byte[] SrcArray)
        {
            fixed (byte* Packet = SrcArray)
                *(uint*)(Packet + Offset) = Value;
            return SrcArray;
        }
        public static byte[] Fill(string arg, int offset, byte[] buffer)
        {
            ushort i = 0;
            while (i < arg.Length)
            {
                buffer[(ushort)(i + offset)] = (byte)arg[i];
                i = (ushort)(i + 1);
            }
            return buffer;
        }
    }
}