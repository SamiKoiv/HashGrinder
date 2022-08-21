using System.Security.Cryptography;

namespace HashGrinder
{
    internal class Hash_SHA256 : IHash
    {
        public byte[] Hash(byte[] bytes)
        {
            var hasher = SHA256.Create();
            var hash = hasher.ComputeHash(bytes);
            return hash;
        }
    }
}
