using HashGrinder;
using System.Diagnostics;

IHash hasher = new Hash_SHA256();

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
var hash = new byte[1];
var matchFound = false;

var timer = new Stopwatch();
var totalTimer = Stopwatch.StartNew();

// Generate array with extending length
for (int i = 1; i <= maxLength; i++)
{
    bytes = new byte[i];
    UInt64 maxChanges = Convert.ToUInt64(Math.Pow(byte.MaxValue, i));

    Console.WriteLine();
    Console.WriteLine($"Iteration {i}, {maxChanges} individual values");
    timer.Restart();

    // Assign values
    for (UInt64 j = 0; j <= maxChanges; j++)
    {
        // Over the cell max value
        if (bytes[0] == byte.MaxValue)
        {
            // Iterate through byte array to resolve the increment
            for (int h = 1; h < i; h++)
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

        hash = hasher.Hash(bytes);
        matchFound = hash.Equals(promptBytes);

        if (matchFound)
            break;

        bytes[0]++;
    }

    if (matchFound)
        break;

    Console.WriteLine($"DONE ({timer.ElapsedMilliseconds} ms)");
    Console.WriteLine();
}

timer.Stop();

Console.Write($"Prompt: {Convert.ToHexString(promptBytes)}");
Console.WriteLine();

Console.Write($"FirstMatch: {Convert.ToHexString(hash)}");
Console.WriteLine();

Console.WriteLine($"FINISHED in {totalTimer.ElapsedMilliseconds} ms");
Console.ReadLine();