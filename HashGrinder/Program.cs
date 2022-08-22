using HashGrinder;
using System.Diagnostics;

IHasher hasher = new Hash_SHA256();
HashRootFinder finder = new(hasher);
SeedGenerator seedGenerator = new();
byte[] seed;
byte[] target;
int roundCount;

Console.WriteLine("------------------");
Console.WriteLine("|| HASH GRINDER ||");
Console.WriteLine("------------------");

// Ask for round count
while (true)
{
    Console.Write("Enter round count: ");
    if (int.TryParse(Console.ReadLine(), out roundCount))
        break;

    Console.WriteLine();
}

// First round target
seed = seedGenerator.Random(1);

// Run cycles
for (int round = 1; round <= roundCount; round++)
{
    Console.WriteLine();
    target = hasher.Hash(seed);

    ShowInfo.RoundStart(round, seed, target);
    var cycleTimer = Stopwatch.StartNew();

    const byte maxLength = byte.MaxValue;

    byte[]? firstMatch = null;

    // Generate array with extending length
    for (int i = 1; i <= maxLength; i++)
    {
        firstMatch = finder.FindRoot(i, target);

        if (firstMatch != null)
            break;
    }

    cycleTimer.Stop();
    ShowInfo.RoundResults(seed, target, firstMatch, cycleTimer.ElapsedMilliseconds);

    if (firstMatch == null)
        return;

    // Generate seed for the next round
    seed = seedGenerator.Merge(target, firstMatch);
}

Console.WriteLine();
Console.WriteLine("END");
Console.ReadLine();