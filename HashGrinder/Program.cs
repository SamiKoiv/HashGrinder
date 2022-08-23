using HashGrinder;
using HashGrinder.Hashers;
using HashGrinder.HashRootFinders;
using System.Diagnostics;

IHasher hasher = new Hasher_SHA256();
IHashRootFinder finder = new HashRootFinder_MultiThreaded(hasher);
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

// Run rounds
for (int round = 1; round <= roundCount; round++)
{
    Console.WriteLine();
    target = hasher.Hash(seed);

    ShowInfo.RoundStart(round, seed, target);
    var cycleTimer = Stopwatch.StartNew();

    var firstMatch = finder.FindRoot(target);

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