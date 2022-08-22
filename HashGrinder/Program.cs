using HashGrinder;
using System.Diagnostics;

IHasher hasher = new Hash_SHA256();
HashRootFinder finder = new(hasher);

int roundCount;

while (true)
{
    Console.Write("Enter round count: ");
    if (int.TryParse(Console.ReadLine(), out roundCount))
        break;

    Console.WriteLine();
}

// First round prompt
var seed = new byte[1];
new Random().NextBytes(seed);
Console.WriteLine($"First target seed: {Convert.ToHexString(seed)}");
var prompt = hasher.Hash(seed);

// Prompt validation
if (prompt.Length % 2 != 0)
{
    throw new ArgumentException("Hex prompt cannot have odd number of characters");
}

Console.WriteLine();

// Run cycles
for (int round = 1; round <= roundCount; round++)
{
    Console.WriteLine($"--- ROUND {round} ---");
    Console.WriteLine($"Target: {Convert.ToHexString(prompt)}");
    var cycleTimer = Stopwatch.StartNew();

    const byte maxLength = byte.MaxValue;
    //const byte maxLength = 3;

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
    Console.WriteLine($"Prompt: {Convert.ToHexString(prompt)}");

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
    var newPrompt = new byte[prompt.Length + firstMatch.Length];

    for (int i = 0; i < prompt.Length; i++)
        newPrompt[i] = prompt[i];

    for (var i = 0; i < firstMatch.Length; i++)
        newPrompt[prompt.Length - 1 + i] = firstMatch[i];

    prompt = hasher.Hash(newPrompt);
}
