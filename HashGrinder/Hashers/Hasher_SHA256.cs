using System.Security.Cryptography;

namespace HashGrinder.Hashers
{
    internal class Hasher_SHA256 : IHasher
    {
        public byte[] Hash(byte[] bytes)
        {
            var hasher = SHA256.Create();
            var hash = hasher.ComputeHash(bytes);
            return hash;
        }
    }
}
