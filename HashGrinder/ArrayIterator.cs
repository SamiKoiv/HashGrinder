using System.Diagnostics;

namespace HashGrinder
{
    internal class HashRootFinder
    {
        private readonly IHasher _hasher;

        public HashRootFinder(IHasher hasher)
        {
            _hasher = hasher;
        }

        public byte[]? FindRoot(int length, byte[] reference)
        {
            var timer = Stopwatch.StartNew();

            var bytes = new byte[length];
            UInt64 maxChanges = Convert.ToUInt64(Math.Pow(byte.MaxValue, bytes.Length));

            Console.WriteLine();
            Console.WriteLine($"Iteration {length}, {maxChanges} individual values");

            byte[] hash;
            bool matchFound;

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

                hash = _hasher.Hash(bytes);
                matchFound = hash.Equals(reference);

                if (matchFound)
                    return bytes;

                bytes[0]++;
            }

            timer.Stop();

            Console.WriteLine($"DONE ({timer.ElapsedMilliseconds} ms)");
            Console.WriteLine();

            return null;
        }

    }
}
