using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpXDecrypt
{
    class RC4
    {
        public static byte[] Encrypt(byte[] pwd, byte[] data)
        {
            int[] array = new int[256];
            int[] array2 = new int[256];
            byte[] array3 = new byte[data.Length];
            int i;
            for (i = 0; i < 256; i++)
            {
                array[i] = pwd[i % pwd.Length];
                array2[i] = i;
            }
            int num = i = 0;
            for (; i < 256; i++)
            {
                num = (num + array2[i] + array[i]) % 256;
                int num2 = array2[i];
                array2[i] = array2[num];
                array2[num] = num2;
            }
            int num3 = num = (i = 0);
            for (; i < data.Length; i++)
            {
                num3++;
                num3 %= 256;
                num += array2[num3];
                num %= 256;
                int num2 = array2[num3];
                array2[num3] = array2[num];
                array2[num] = num2;
                int num4 = array2[(array2[num3] + array2[num]) % 256];
                array3[i] = (byte)(data[i] ^ num4);
            }
            return array3;
        }

        public static byte[] Decrypt(byte[] pwd, byte[] data)
        {
            return Encrypt(pwd, data);
        }
    }
}
