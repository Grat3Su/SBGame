using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System;

namespace STD
{
    //���� �뷮�̶� �ű�� ������ ������ �������� �ӵ��� ���ϵ�
    public class FileIO//������ �����
    {
        public static void save(string path, byte[] bytes)//�������� �����ؾ� �� �� �ִ�.
        {
            //File* pf = fopen(path, "wb);
            FileStream fs = File.Open(path, FileMode.OpenOrCreate);//���� start
            fs.Write(bytes, 0, bytes.Length);//����Ʈ�� �ѱ� �����͸� ��� ����
            fs.Close();//���� end
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

            IntPtr ptr = Marshal.AllocHGlobal(len);//�Ҵ�
            Marshal.StructureToPtr(obj, ptr, false);//obj�� ptr�� �ű�/ �߰� ����
            Marshal.Copy(ptr, bytes, 0, len);//prt�� pytes�� ����
            Marshal.FreeHGlobal(ptr);//����
            return bytes;
        }

        //bytes2object        
        //public static object bytes2object(byte[] bytes) where : struct
        public static T bytes2object<T>(byte[] bytes) where T : struct//����ü�� Ÿ������ �޴´�
        {
            int len = Marshal.SizeOf(typeof(T));

            if (bytes.Length < len)
            {
                //T obj = null;
                //return null;
            }

            IntPtr ptr = Marshal.AllocHGlobal(len);//�Ҵ�            
            Marshal.Copy(bytes, 0, ptr, len);//bytes�� ptr�� ����
            T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);//����

            return obj;
        }
    }
}