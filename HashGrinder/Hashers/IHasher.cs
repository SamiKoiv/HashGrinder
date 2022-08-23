namespace HashGrinder.Hashers
{
    internal interface IHasher
    {
        byte[] Hash(byte[] bytes);
    }
}
