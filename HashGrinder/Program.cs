using HashGrinder;
using System.Diagnostics;

IHasher hasher = new Hash_SHA256();
HashRootFinder finder = new(hasher);
SeedGenerator seedGenerator = new();
byte[] prompt;
int roundCount;

// Ask for round count
while (true)
{
    Console.Write("Enter round count: ");
    if (int.TryParse(Console.ReadLine(), out roundCount))
        break;

    Console.WriteLine();
}
Console.WriteLine();

// First round prompt
var seed = seedGenerator.Random(1);

// Run cycles
for (int round = 1; round <= roundCount; round++)
{
    prompt = hasher.Hash(seed);

    Console .WriteLine($"--- ROUND {round} ---");
    Console.WriteLine($"Seed: {Convert.ToHexString(seed)}");
    Console.WriteLine($"Target: {Convert.ToHexString(prompt)}");
    var cycleTimer = Stopwatch.StartNew();

    const byte maxLength = byte.MaxValue;

    byte[]? firstMatch = null;

    // Generate array with extending length
    for (int i = 1; i <= maxLength; i++)
    {
        firstMatch = finder.FindRoot(i, prompt);

        if (firstMatch != null)
            break;
    }

    cycleTimer.Stop();
    Console.WriteLine();
    Console.WriteLine($"--- FINISHED ---");
    Console.WriteLine($"Target: {Convert.ToHexString(prompt)}");

    if (firstMatch == null)
    {
        Console.WriteLine($"First match: Not found.");
        Console.WriteLine($"Time: {cycleTimer.ElapsedMilliseconds} ms");
        return;
    }

    Console.WriteLine($"First match: {Convert.ToHexString(firstMatch)}");

    Console.WriteLine($"Time: {cycleTimer.ElapsedMilliseconds} ms");
    Console.ReadLine();

    // Generate prompt for the next round
    seed = seedGenerator.Merge(prompt, firstMatch);
}
