namespace HashGrinder
{
    internal static class Extensions
    {
        public static byte[] HexToBytes(this string value)
        {
            var arrayLength = value.Length / 2;
            var bytes = new byte[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
                var snippet = value.Skip(i * 2).Take(2).ToArray();
                var stringValue = new string(snippet);
                var byteValue = Convert.ToByte(stringValue, 16);
                bytes[i] = byteValue;
            }

            return bytes;
        }
    }
}
