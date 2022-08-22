namespace HashGrinder
{
    internal static class ShowInfo
    {
        public static void RoundStart(int round, byte[] seed, byte[] target)
        {
            Console.WriteLine($"--- ROUND {round} ---");
            Console.WriteLine($"Seed: {Convert.ToHexString(seed)}");
            Console.WriteLine($"Target: {Convert.ToHexString(target)}");
        }

        public static void RoundResults(byte[] seed, byte[] target, byte[]? firstMatch, long elapsedMilliseconds)
        {
            Console.WriteLine();
            Console.WriteLine($"--- FINISHED ---");
            Console.WriteLine($"Target: {Convert.ToHexString(target)}");

            if (firstMatch == null)
                Console.WriteLine($"First match: Not found.");
            else
                Console.WriteLine($"First match: {Convert.ToHexString(firstMatch)}");

            Console.WriteLine($"Time: {elapsedMilliseconds} ms");
        }
    }
}
