using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System;

namespace STD
{
    //같은 용량이라도 옮기는 파일이 많으면 많을수록 속도가 저하됨
    public class FileIO//ㅍ파일 입출력
    {
        public static void save(string path, byte[] bytes)//여러개를 저장해야 할 수 있다.
        {
            //File* pf = fopen(path, "wb);
            FileStream fs = File.Open(path, FileMode.OpenOrCreate);//권한 start
            fs.Write(bytes, 0, bytes.Length);//바이트로 넘긴 데이터를 모두 저장
            fs.Close();//권한 end
        }

        public static byte[] load(string path)
        {
            if (!File.Exists(path))
                return null;
            FileStream fs = File.Open(path, FileMode.Open);
            //if (fs == null)
            //    return null;
            long len = fs.Length;
            byte[] bytes = new byte[len];
            fs.Read(bytes, 0, (int)len);
            fs.Close();
            return bytes;
        }

        //object2bytes

        public static byte[] struct2bytes(object obj)
        {
            int len = Marshal.SizeOf(obj);
            byte[] bytes = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);//할당
            Marshal.StructureToPtr(obj, ptr, false);//obj를 ptr로 옮김/ 중간 역할
            Marshal.Copy(ptr, bytes, 0, len);//prt을 pytes로 복사
            Marshal.FreeHGlobal(ptr);//해제
            return bytes;
        }

        //bytes2object        
        //public static object bytes2object(byte[] bytes) where : struct
        public static T bytes2object<T>(byte[] bytes) where T : struct//구조체의 타입으로 받는다
        {
            int len = Marshal.SizeOf(typeof(T));

            if (bytes.Length < len)
            {
                //T obj = null;
                //return null;
            }

            IntPtr ptr = Marshal.AllocHGlobal(len);//할당            
            Marshal.Copy(bytes, 0, ptr, len);//bytes를 ptr로 변경
            T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);//해제

            return obj;
        }
    }
}