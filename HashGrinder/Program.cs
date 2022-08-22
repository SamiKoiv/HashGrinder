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

    ShowInfo.RoundStart(round, seed, prompt);
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
    ShowInfo.RoundResults(seed, prompt, firstMatch, cycleTimer.ElapsedMilliseconds);

    if (firstMatch == null)
        return;

    // Generate prompt for the next round
    seed = seedGenerator.Merge(prompt, firstMatch);
}
