using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGrinder
{
    internal static class ShowInfo
    {
        public static void RoundStart(int round, byte[] seed, byte[] prompt)
        {
            Console.WriteLine($"--- ROUND {round} ---");
            Console.WriteLine($"Seed: {Convert.ToHexString(seed)}");
            Console.WriteLine($"Target: {Convert.ToHexString(prompt)}");
        }

        public static void RoundResults(byte[] seed, byte[] prompt, byte[]? firstMatch, long elapsedMilliseconds)
        {
            Console.WriteLine();
            Console.WriteLine($"--- FINISHED ---");
            Console.WriteLine($"Target: {Convert.ToHexString(prompt)}");

            if (firstMatch == null)
            {
                Console.WriteLine($"First match: Not found.");
                Console.WriteLine($"Time: {elapsedMilliseconds} ms");
                return;
            }

            Console.WriteLine($"First match: {Convert.ToHexString(firstMatch)}");

            Console.WriteLine($"Time: {elapsedMilliseconds} ms");
            Console.ReadLine();
        }
    }
}
