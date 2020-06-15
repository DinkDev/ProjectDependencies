namespace ProjectDependencies.Model
{
    using System;
    using System.Security.Cryptography;

    public sealed class Crc32 : HashAlgorithm
    {
        public const uint DefaultPolynomial =  0xedb88320;

        public uint Polynomial { get; }


        public Crc32(uint polynomial = DefaultPolynomial)
        {
            Polynomial = polynomial;

            Initialize();
        }

        public override void Initialize()
        {
            Table = InitializeTable();
            Accumulator = DefaultSeed;
        }

        public override int HashSize => 32;

        public uint Accumulator { get; private set; }

        public uint[] Table { get; set; }

        public uint DefaultSeed => 0xffffffff;

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            Accumulator = CalculateHash(Table, Accumulator, buffer, Convert.ToUInt32(start), Convert.ToUInt32(length));
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = UintToBytes(~Accumulator);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        private uint[] InitializeTable()
        {
            var rv = new uint[256];
            for (var i = 0U; i < 256U; i++)
            {
                var entry = i;
                for (var j = 0U; j < 8U; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ Polynomial;
                    }
                    else
                    {
                        entry >>= 1;
                    }
                }

                rv[i] = entry;
            }

            return rv;
        }

        private static uint CalculateHash(uint[] table, uint seed, byte[] buffer, uint start, uint size)
        {
            var crc = seed;

            for (var i = start; i < size; i++)
            {
                crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
            }

            return crc;
        }

        public byte[] UintToBytes(uint x)
        {
            return new[]
            {
                (byte)((x >> 24) & 0xff),
                (byte)((x >> 16) & 0xff),
                (byte)((x >> 8) & 0xff),
                (byte)(x & 0xff)
            };
        }

        public uint BytesToUint(byte[] x)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (x.Length != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(x), @"Must be 4 bytes");
            }

            var rv = 0u;

            rv += Convert.ToUInt32(x[0]) << 24;
            rv += Convert.ToUInt32(x[1]) << 16;
            rv += Convert.ToUInt32(x[2]) << 8;
            rv += Convert.ToUInt32(x[3]);

            return rv;
        }
    }
}
