using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGrinder
{
    internal class ArrayIterator
    {
        private readonly IHash _hasher;

        public ArrayIterator(IHash hasher)
        {
            _hasher = hasher;
        }

        public byte[]? Iterate(int length, byte[] reference)
        {
            var bytes = new byte[length];
            UInt64 maxChanges = Convert.ToUInt64(Math.Pow(byte.MaxValue, bytes.Length));

            Console.WriteLine();
            Console.WriteLine($"Iteration {length}, {maxChanges} individual values");

            // Assign values
            for (UInt64 j = 0; j <= maxChanges; j++)
            {
                // Over the cell max value
                if (bytes[0] == byte.MaxValue)
                {
                    // Iterate through byte array to resolve the increment
                    for (int h = 1; h < bytes.Length; h++)
                    {
                        if (bytes[h] == byte.MaxValue)
                        {
                            bytes[h] = 0;
                            continue;
                        }

                        bytes[h]++;
                    }

                    bytes[0] = 0;
                    continue;
                }

                var hash = _hasher.Hash(bytes);
                var matchFound = hash.Equals(reference);

                if (matchFound)
                    return bytes;

                bytes[0]++;
            }

            return null;
        }

    }
}
