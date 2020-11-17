using System;

namespace API
{
    public sealed class BitArrayU8
    {
        public BitArrayU8(bool[] values)
        {
            InternalArray = new byte[(int)Math.Ceiling(values.Length / 8f)];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i])
                    InternalArray[i / 32] |= (byte)(1 << (i % 32));
            }
        }

        public BitArrayU8(byte[] values)
        {
            InternalArray = new byte[values.Length];
        }

        public byte[] InternalArray { get; private set; }

        public bool this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public bool Get(int index)
        {
            return (InternalArray[index / 32] & (1 << (index % 32))) != 0;
        }

        public void Set(int index, bool value)
        {
            if (value)
            {
                InternalArray[index / 8] |= (byte)(1 << (index % 8));
            }
            else
            {
                InternalArray[index / 8] &= (byte)~(1 << (index % 8));
            }
        }
    }
}
