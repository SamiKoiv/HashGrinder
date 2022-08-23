using System.Diagnostics;
using HashGrinder.Hashers;

namespace HashGrinder.HashRootFinders
{
    internal class HashRootFinder : IHashRootFinder
    {
        private readonly IHasher _hasher;

        public HashRootFinder(IHasher hasher)
        {
            _hasher = hasher;
        }

        public byte[]? FindRoot(byte[] reference, int length)
        {
            var timer = Stopwatch.StartNew();
            var processSeconds = 0;

            var bytes = new byte[length];
            ulong maxChanges = Convert.ToUInt64(Math.Pow(byte.MaxValue, bytes.Length));

            Console.WriteLine();
            Console.WriteLine($"Iteration {length}, {maxChanges} individual values");

            byte[] hash;
            bool matchFound;

            // Assign values
            for (ulong j = 0; j <= maxChanges; j++)
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

                matchFound = true;
                for (int h = 1; h < hash.Length; h++)
                {
                    if (hash[h] != reference[h])
                    {
                        matchFound = false;
                        break;
                    }
                }

                if (matchFound)
                    return bytes;

                bytes[0]++;

                // Output progress info once per second
                var seconds = (int)(timer.ElapsedMilliseconds * 0.001);
                if (seconds != processSeconds)
                {
                    var progress = (double)j / maxChanges * 100;
                    progress = Math.Round(progress, 2);
                    Console.WriteLine($"{progress}% \t {j} / {maxChanges}");
                    processSeconds = seconds;
                }
            }

            timer.Stop();

            Console.WriteLine($"DONE ({timer.ElapsedMilliseconds} ms)");
            Console.WriteLine();

            return null;
        }

    }
}
