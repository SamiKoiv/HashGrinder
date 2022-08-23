namespace HashGrinder.HashRootFinders
{
    internal interface IHashRootFinder
    {
        byte[]? FindRoot(byte[] reference);
    }
}
