using HashGrinder;
using System.Diagnostics;

IHash hasher = new Hash_SHA256();
ArrayIterator iterator = new(hasher);

var testPrompt = "FF1151AFB20E9E4564838EF8075846449287597A4CDD7F598AFF8B36231792B1";

if (testPrompt.Length % 2 != 0)
{
    throw new ArgumentException("Hex prompt cannot have odd number of characters");
}

var arrayLength = testPrompt.Length / 2;
var promptBytes = new byte[arrayLength];

for (int i = 0; i < arrayLength; i++)
{
    var value = testPrompt.Skip(i * 2).Take(2).ToArray();
    var stringValue = new string(value);
    var byteValue = Convert.ToByte(stringValue, 16);
    promptBytes[i] = byteValue;
}

//const byte maxLength = byte.MaxValue;
const byte maxLength = 3;
byte[] bytes = new byte[1];

var timer = new Stopwatch();
var totalTimer = Stopwatch.StartNew();

byte[]? firstMatch = null;

// Generate array with extending length
for (int i = 1; i <= maxLength; i++)
{
    timer.Restart();
    firstMatch = iterator.Iterate(i, promptBytes);

    if (firstMatch != null)
        break;

    Console.WriteLine($"DONE ({timer.ElapsedMilliseconds} ms)");
    Console.WriteLine();
}

timer.Stop();

Console.Write($"Prompt: {Convert.ToHexString(promptBytes)}");
Console.WriteLine();

if (firstMatch != null)
    Console.Write($"FirstMatch: {Convert.ToHexString(firstMatch)}");
else
    Console.Write($"No match found.");
Console.WriteLine();


Console.WriteLine($"FINISHED in {totalTimer.ElapsedMilliseconds} ms");
Console.ReadLine();