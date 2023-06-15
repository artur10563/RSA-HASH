using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSALIB
{
    public class HashFunction
    {
        private static ulong[] iv = {
                 12345678901234567890,
                 9876543210987654321,
                 5555555555555555555,
                 9999999999999999999 };

        private const int sizeOfBlockHash = 16; //128 bit
        byte[][] Blocks;

        private byte[] ByteToRightLength(byte[] input)
        {
            int AddOfByte = 0;
            if ((input.Length % sizeOfBlockHash) != 0)
            {
                AddOfByte = sizeOfBlockHash - input.Length % sizeOfBlockHash;
                Array.Resize(ref input, input.Length + AddOfByte);
            }
            return input;
        }

        private void CutByteIntoBlocks(byte[] input)
        {
            int NumberOfBlock = input.Length / sizeOfBlockHash;
            Blocks = new byte[NumberOfBlock][];

            for (int i = 0; i < NumberOfBlock; i++) Blocks[i] = new byte[sizeOfBlockHash];

            for (int i = 0; i < NumberOfBlock; i++)
                Array.Copy(input, i * sizeOfBlockHash, Blocks[i], 0, sizeOfBlockHash);
        }

        public byte[] CreateHashCode(string InFile, string OutFile)
        {
            byte[] array;
            byte[] HashCode = new byte[iv.Length * 4];
            RSA.RSApkde pkde;
            pkde.RSAByte1 = 0;
            pkde.RSAByte2 = 0;
            pkde.RSAByte3 = 0;
            pkde.RSAByte4 = 0;

            for (int i = 0; i < iv.Length; i++)
            {
                pkde.RSAInt = iv[i];
                HashCode[i * 4] = pkde.RSAByte1;
                HashCode[i * 4 + 1] = pkde.RSAByte2;
                HashCode[i * 4 + 2] = pkde.RSAByte3;
                HashCode[i * 4 + 3] = pkde.RSAByte4;
            }

            using (FileStream fstream = File.OpenRead(@InFile))
            {
                array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
            }

            array = ByteToRightLength(array);
            CutByteIntoBlocks(array);

            for (int i = 0; i < Blocks.Length; i++)
                for (int j = 0; j < Blocks[i].Length; j++)
                    HashCode[j] = (byte)((int)Blocks[i][j] ^ (int)HashCode[j]);
            using (FileStream fstream = new FileStream(@OutFile, FileMode.Create))
            {
                fstream.Write(HashCode, 0, HashCode.Length);
            }
            return HashCode;

        }


    }
}
