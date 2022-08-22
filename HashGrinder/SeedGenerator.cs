namespace HashGrinder
{
    internal class SeedGenerator
    {
        public byte[] Random(int length)
        {
            var seed = new byte[length];
            new Random().NextBytes(seed);
            return seed;
        }

        public byte[] Merge(byte[] a, byte[] b)
        {
            var seed = new byte[a.Length + b.Length];

            for (int i = 0; i < a.Length; i++)
                seed[i] = a[i];

            for (var i = 0; i < b.Length; i++)
                seed[a.Length + i] = b[i];

            return seed;
        }
    }
}
