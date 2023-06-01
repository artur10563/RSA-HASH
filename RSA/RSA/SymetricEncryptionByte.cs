using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSALIB
{
    public class SymetricEncryptionByte
    {
        private const int sizeOfBlock = 16; //128 bit
        private static int AddOfByte = 0;
        private const int shiftKey = 2;
        private static int quantityOfRounds = 16;
        byte[][] Blocks;


        private byte[] ByteToRightLength(byte[] input)
        {
            AddOfByte = 0;
            if ((input.Length % sizeOfBlock) != 0)
            {
                AddOfByte = sizeOfBlock - input.Length % sizeOfBlock;
                Array.Resize(ref input, input.Length + AddOfByte);
            }
            return input;
        }

        private byte[] ByteAllToBit(byte[] input)
        {
            byte[] bit = new byte[input.Length * 8];
            void ByteOneToBit(byte one, int k)
            {
                for (int i = 0; i < sizeof(byte) * 8; i++)
                {
                    if ((one & (1 << i)) != 0)
                        bit[k * 8 - i + 7] = 1;
                    else
                        bit[k * 8 - i + 7] = 0;
                }
            }

            for (int k = 0; k < input.Length; k++) ByteOneToBit((byte)input[k], k);

            return bit;
        }

        private byte[] BitAllToByte(byte[] input)
        {
            byte[] bytes = new byte[input.Length / (sizeof(byte) * 8)];
            byte[] temp = new byte[sizeof(byte) * 8];
            void BitsToOneByte(byte[] one, int k)
            {
                byte n = 1, b = 0;
                for (int i = sizeof(byte) * 8 - 1; i >= 0; i--, n *= 2) b += (byte)(one[i] * n);
                bytes[k] = b;
            }
            for (int k = 0; k < input.Length / 8; k++)
            {
                Array.Copy(input, k * 8, temp, 0, 8);
                BitsToOneByte(temp, k);
            }
            return bytes;
        }

        private void CutByteIntoBlocks(byte[] input)
        {
            int NumberOfBlock = input.Length / sizeOfBlock;
            Blocks = new byte[NumberOfBlock][];

            for (int i = 0; i < NumberOfBlock; i++) Blocks[i] = new byte[sizeOfBlock];
            for (int i = 0; i < NumberOfBlock; i++) Array.Copy(input, i * sizeOfBlock, Blocks[i], 0, sizeOfBlock);

        }

        private byte[] CorrectKeyWord(byte[] input, int lengthKey)
        {
            Array.Resize(ref input, lengthKey);
            return input;
        }

        private byte[] XOR(byte[] b1, byte[] b2)
        {
            byte[] result = new byte[b1.Length];

            for (int i = 0; i < b1.Length; i++) result[i] = (byte)(((int)b1[i] ^ (int)b2[i]));
            return result;
        }

        private byte[] f(byte[] b1, byte[] b2)
        {
            return XOR(b1, b2);
        }

        private byte[] EncodeDes_One_Round(byte[] input, byte[] key)
        {
            int k = input.Length / 2;

            byte[] temp = new byte[k];
            byte[] result = new byte[input.Length];
            byte[] L = new byte[k];
            byte[] R = new byte[k];
            Array.Copy(input, 0, L, 0, k);
            Array.Copy(input, k, R, 0, k);

            temp = XOR(L, f(R, key));

            Array.Copy(R, 0, result, 0, k);
            Array.Copy(temp, 0, result, k, k);
            return result;

        }

        private byte[] DecodeDES_One_Round(byte[] input, byte[] key)
        {
            int k = input.Length / 2;

            byte[] temp = new byte[k];
            byte[] result = new byte[input.Length];
            byte[] L = new byte[k];
            byte[] R = new byte[k];
            Array.Copy(input, 0, L, 0, k);
            Array.Copy(input, k, R, 0, k);

            temp = XOR(f(L, key), R);
            Array.Copy(temp, 0, result, 0, k);
            Array.Copy(L, 0, result, k, k);
            return result;
        }

        private byte[] KeyToNextRound(byte[] key)
        {
            byte[] bits = ByteAllToBit(key);

            byte first;
            for (int i = 0; i < shiftKey; i++)
            {
                first = bits[bits.Length - 1];
                for (int j = bits.Length - 1; j > 0; j--) bits[j] = bits[j - 1];
                bits[0] = first;
            }

            key = BitAllToByte(bits);
            return key;
        }

        private byte[] KeyToPrevRound(byte[] key)
        {
            byte[] bits = ByteAllToBit(key);
            byte first;

            for (int i = 0; i < shiftKey; i++)
            {
                first = bits[0];
                for (int j = 0; j < bits.Length - 1; j++) bits[j] = bits[j + 1];
                bits[bits.Length - 1] = first;
            }
            key = BitAllToByte(bits);
            return key;
        }

        public byte[] UlongToByte(long k)
        {
            byte[] key = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                key[3 - i] = (byte)((k & (255 << i * 8)) >> i * 8);
            }
            return key;
        }

        public byte[] CorrectKeyForServerB(byte[] key)
        {
            byte[] DecodeKey = new byte[key.Length];
            key = CorrectKeyWord(key, sizeOfBlock / 2);

            for (int j = 0; j < quantityOfRounds; j++)
                key = KeyToNextRound(key);

            DecodeKey = KeyToPrevRound(key);

            return DecodeKey;
        }

        public byte[] EncryptFile(string InFile, string OutFile, byte[] key, out int AddByte)
        {
            if (key.Length > 0)
            {
                byte[] DecodeKey = new byte[key.Length];
                byte[] array;

                using (FileStream fstream = File.OpenRead(@InFile))
                {
                    array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                }
                array = ByteToRightLength(array);

                CutByteIntoBlocks(array);

                key = CorrectKeyWord(key, sizeOfBlock / 2);
                for (int j = 0; j < quantityOfRounds; j++)
                {
                    for (int i = 0; i < Blocks.Length; i++)
                        Blocks[i] = EncodeDes_One_Round(Blocks[i], key);
                    key = KeyToNextRound(key);
                }

                DecodeKey = KeyToPrevRound(key);

                byte[] result = new byte[Blocks.Length * Blocks[0].Length];

                for (int i = 0; i < Blocks.Length; i++)
                    for (int j = 0; j < Blocks[i].Length; j++) result[i * Blocks[0].Length + j] = Blocks[i][j];

                using (FileStream fstream = new FileStream(@OutFile, FileMode.OpenOrCreate))
                {
                    fstream.Write(result, 0, result.Length);
                }
                AddByte = AddOfByte;
                return DecodeKey;
            }
            AddByte = AddOfByte;
            return null;
        }

        public void DecipherFile(string InFile, string OutFile, byte[] DecodeKey, int AddByte)
        {
            if (DecodeKey.Length > 0)
            {
                byte[] array;

                using (FileStream fstream = File.OpenRead(@InFile))
                {
                    array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                }

                CutByteIntoBlocks(array);

                for (int j = 0; j < quantityOfRounds; j++)
                {
                    for (int i = 0; i < Blocks.Length; i++)
                        Blocks[i] = DecodeDES_One_Round(Blocks[i], DecodeKey);
                    DecodeKey = KeyToPrevRound(DecodeKey);
                }

                DecodeKey = KeyToNextRound(DecodeKey);
                byte[] result = new byte[Blocks.Length * Blocks[0].Length];

                for (int i = 0; i < Blocks.Length; i++)
                    for (int j = 0; j < Blocks[i].Length; j++) result[i * Blocks[0].Length + j] = Blocks[i][j];

                using (FileStream fstream = new FileStream(@OutFile, FileMode.OpenOrCreate))
                {
                    fstream.Write(result, 0, result.Length - AddByte);
                }
            }
        }

    }
}
