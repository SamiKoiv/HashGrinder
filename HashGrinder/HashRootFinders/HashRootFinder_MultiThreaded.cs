using HashGrinder.Hashers;
using System.Diagnostics;

namespace HashGrinder.HashRootFinders
{
    internal class HashRootFinder_MultiThreaded : IHashRootFinder
    {
        private readonly IHasher _hasher;

        public HashRootFinder_MultiThreaded(IHasher hasher)
        {
            _hasher = hasher;
        }

        public byte[]? FindRoot(byte[] reference)
        {
            const byte maxLength = byte.MaxValue;
            byte[]? firstMatch = null;

            // Generate array with extending length
            for (int i = 1; i <= maxLength; i++)
            {
                firstMatch = RunArray(reference, i);

                if (firstMatch != null)
                    break;
            }

            return firstMatch;
        }

        public byte[]? RunArray(byte[] reference, int length)
        {
            var result = FindRootThreaded(length, reference, 1).Result;
            return result;
        }

        private async Task<byte[]?> FindRootThreaded(int length, byte[] reference, uint threadCount)
        {
            var tasks = new List<Task<byte[]?>>();

            for (ulong i = 0; i < (ulong)threadCount; i++)
            {
                tasks.Add(Task.Run(() => FindRoot(length, reference, i, (ulong)threadCount)));
            }

            var results = await Task.WhenAll(tasks);
            var firstResult = results.FirstOrDefault(x => x != null);

            //var result = await Task.Run(() => FindRoot(length, reference, offset, increment));
            return firstResult;
        }

        private byte[]? FindRoot(int length, byte[] reference, ulong offset, ulong increment)
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
            for (ulong j = offset; j <= maxChanges; j += increment)
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
